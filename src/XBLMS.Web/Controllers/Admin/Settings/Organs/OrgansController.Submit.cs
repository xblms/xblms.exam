using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Organs
{
    public partial class OrgansController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] GetSubmitRequest request)
        {
            if (request.Id > 0)
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
                {
                    return this.NoAuth();
                }
            }
            else
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
                {
                    return this.NoAuth();
                }
            }
            var admin = await _authManager.GetAdminAsync();

            if (request.Type == "company")
            {
                if (request.Id > 0)
                {
                    var last = await _companyRepository.GetAsync(request.Id); ;
                    var company = await _companyRepository.GetAsync(request.Id);
                    var oldName = company.Name;
                    company.Name = request.Name;
                    if (!StringUtils.Equals(oldName, request.Name))
                    {
                        var (path, names) = await _companyRepository.GetParentPathAndNamesAsync(company.Id);
                        company.CompanyParentPath = path;
                        company.ParentNames = names;

                        await _companyRepository.UpdateAsync(company);
                        await _authManager.AddAdminLogAsync("修改单位", $"{oldName}>{company.Name}");
                        await _authManager.AddStatLogAsync(StatType.CompanyUpdate, "修改单位", company.Id, company.Name, last);
                    }
                }
                else
                {
                    await AddOrgans(request.Name, request.ParentId, request.Type, request.ParentType, admin.Id);
                }
            }
            if (request.Type == "department")
            {
                if (request.Id > 0)
                {
                    var last = await _organDepartmentRepository.GetAsync(request.Id);
                    var department = await _organDepartmentRepository.GetAsync(request.Id);
                    var oldName = department.Name;
                    department.Name = request.Name;
                    if (!StringUtils.Equals(oldName, request.Name))
                    {
                        var (path, names) = await _organDepartmentRepository.GetParentPathAndNamesAsync(department.Id);
                        department.DepartmentParentPath = path;
                        department.ParentNames = names;

                        await _organDepartmentRepository.UpdateAsync(department);
                        await _authManager.AddAdminLogAsync("修改部门", $"{oldName}>{department.Name}");

                        await _authManager.AddStatLogAsync(StatType.DepartmentUpdate, "修改部门", department.Id, department.Name, last);
                    }
                }
                else
                {
                    await AddOrgans(request.Name, request.ParentId, request.Type, request.ParentType, admin.Id);
                }
            }
            return new BoolResult
            {
                Value = true
            };
        }
        private async Task AddOrgans(string organNames, int organParentId, string type, string parentType, int adminId)
        {
            var config = await _configRepository.GetAsync();
            var adminDefaultPwd = string.IsNullOrEmpty(config.AdminDefaultPassword) ? "password@1" : config.AdminDefaultPassword;

            var companyId = 0;

            var insertedTreeIdHashtable = new Hashtable { [1] = organParentId };

            if (type == "department")
            {
                if (parentType == "company")
                {
                    companyId = organParentId;
                    insertedTreeIdHashtable = new Hashtable { [1] = 0 };
                }
                if (parentType == "department")
                {
                    var findDepartment = await _organDepartmentRepository.GetAsync(organParentId);
                    companyId = findDepartment.CompanyId;
                }
            }

            var names = organNames.Split('\n');
            foreach (var item in names)
            {
                if (string.IsNullOrEmpty(item)) continue;

                var count = StringUtils.GetStartCount("－", item) == 0 ? StringUtils.GetStartCount("-", item) : StringUtils.GetStartCount("－", item);
                var name = item.Substring(count, item.Length - count);
                var fullName = name;
                count++;

                if (!string.IsNullOrEmpty(name) && insertedTreeIdHashtable.Contains(count))
                {
                    if (name.Contains("(") && name.Contains(")"))
                    {
                        var letName = name;
                        var length = name.IndexOf(")") - name.IndexOf("(");
                        if (length > 0)
                        {
                            name = name.Substring(0, name.IndexOf("("));
                            fullName = letName.Substring(name.Length + 1, letName.Length - name.Length - 2);
                        }
                    }
                    name = name.Trim();
                    fullName = fullName.Trim();

                    var parentId = (int)insertedTreeIdHashtable[count];

                    var insertId = 0;
                    if (type == "company")
                    {
                        var company = new OrganCompany()
                        {
                            Name = name,
                            ParentId = parentId,
                            CreatorId = adminId
                        };
                        insertId = await _companyRepository.InsertAsync(company);
                        company = await _companyRepository.GetAsync(insertId);

                        var (path, cnames) = await _companyRepository.GetParentPathAndNamesAsync(company.Id);

                        var adminMaxId = await _administratorRepository.GetMaxId();
                        var adminUserName = StringUtils.PadZeroes(adminMaxId + 1, 8);

                        company.CompanyParentPath = path;
                        company.ParentNames = cnames;

                        await _companyRepository.UpdateAsync(company);

                        await _administratorRepository.InsertAsync(new Administrator
                        {
                            UserName = adminUserName,
                            DisplayName = $"{company.Name}/单位管理员",
                            CompanyId = company.Id,
                            CompanyParentPath = company.CompanyParentPath,
                            Auth = AuthorityType.AdminCompany,
                            AuthData = AuthorityDataType.DataAll,
                            AuthDataCurrentOrganId = company.Id,
                            AuthDataShowAll = true,
                            CreatorId = 1,
                        }, adminDefaultPwd);

                        await _authManager.AddAdminLogAsync("新增单位", name);
                        await _authManager.AddStatLogAsync(StatType.CompanyAdd, "新增单位", company.Id, company.Name);
                        await _authManager.AddStatCount(StatType.CompanyAdd);
                    }
                    if (type == "department")
                    {
                        var department = new OrganDepartment()
                        {
                            ParentId = parentId,
                            Name = name,
                            CompanyId = companyId,
                            DepartmentId = 0,
                            CreatorId = adminId
                        };

                        insertId = await _organDepartmentRepository.InsertAsync(department);

                        department = await _organDepartmentRepository.GetAsync(insertId);

                        var (path, cnames) = await _organDepartmentRepository.GetParentPathAndNamesAsync(department.Id);

                        department.DepartmentParentPath = path;
                        department.ParentNames = cnames;

                        await _organDepartmentRepository.UpdateAsync(department);

                        await _authManager.AddAdminLogAsync("新增部门", name);
                        await _authManager.AddStatLogAsync(StatType.DepartmentAdd, "新增部门", department.Id, department.Name);
                        await _authManager.AddStatCount(StatType.DepartmentAdd);
                    }

                    insertedTreeIdHashtable[count + 1] = insertId;
                }
            }
        }
    }
}
