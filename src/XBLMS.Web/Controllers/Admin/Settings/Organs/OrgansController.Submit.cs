using Microsoft.AspNetCore.Mvc;
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
                        await _companyRepository.UpdateAsync(company);
                        await _authManager.AddAdminLogAsync("修改单位", $"{oldName}>{company.Name}");
                        await _authManager.AddStatLogAsync(StatType.CompanyUpdate, "修改单位", company.Id, company.Name, last);
                    }
                }
                else
                {
                    var company = new OrganCompany()
                    {
                        Name = request.Name,
                        ParentId = request.ParentId,
                        CompanyId = admin.CompanyId,
                        DepartmentId = 0,
                        CreatorId = admin.Id
                    };
                    var companyId = await _companyRepository.InsertAsync(company);
                    company.CompanyId = companyId;
                    await _companyRepository.UpdateAsync(company);

                    await _authManager.AddAdminLogAsync("新增单位", request.Name);
                    await _authManager.AddStatLogAsync(StatType.CompanyAdd, "新增单位", company.Id, company.Name);
                    await _authManager.AddStatCount(StatType.CompanyAdd);
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
                        await _organDepartmentRepository.UpdateAsync(department);
                        await _authManager.AddAdminLogAsync("修改部门", $"{oldName}>{department.Name}");

                        await _authManager.AddStatLogAsync(StatType.DepartmentUpdate, "修改部门", department.Id, department.Name, last);
                    }
                }
                else
                {
                    var parentId = 0;
                    var companyId = 0;
                    if (request.ParentType == "department")
                    {
                        parentId = request.ParentId;
                        var parent = await _organDepartmentRepository.GetAsync(parentId);
                        companyId = parent.CompanyId;
                    }
                    if (request.ParentType == "company")
                    {
                        parentId = 0;
                        var parent = await _companyRepository.GetAsync(request.ParentId);
                        companyId = parent.Id;
                    }

                    var department = new OrganDepartment()
                    {
                        ParentId = parentId,
                        Name = request.Name,
                        CompanyId = companyId,
                        DepartmentId = 0,
                        CreatorId = admin.Id
                    };

                    var departmentId = await _organDepartmentRepository.InsertAsync(department);
                    department.DepartmentId = departmentId;
                    await _organDepartmentRepository.UpdateAsync(department);

                    await _authManager.AddAdminLogAsync("新增部门", request.Name);
                    await _authManager.AddStatLogAsync(StatType.DepartmentAdd, "新增部门", department.Id, department.Name);
                    await _authManager.AddStatCount(StatType.DepartmentAdd);
                }
            }
            if (request.Type == "duty")
            {
                if (request.Id > 0)
                {
                    var last = await _organDutyRepository.GetAsync(request.Id);
                    var duty = await _organDutyRepository.GetAsync(request.Id);
                    var oldName = duty.Name;
                    duty.Name = request.Name;
                    if (!StringUtils.Equals(oldName, request.Name))
                    {
                        await _organDutyRepository.UpdateAsync(duty);
                        await _authManager.AddAdminLogAsync("修改岗位", $"{oldName}>{duty.Name}");
                        await _authManager.AddStatLogAsync(StatType.DutyUpdate, "修改岗位", duty.Id, duty.Name, last);
                    }
                }
                else
                {
                    var parentId = 0;
                    var departmentId = 0;
                    var companyId = 0;
                    if (request.ParentType == "duty")
                    {
                        parentId = request.ParentId;
                        var parent = await _organDutyRepository.GetAsync(parentId);
                        departmentId = parent.DepartmentId;
                        companyId = parent.CompanyId;
                    }
                    if (request.ParentType == "department")
                    {
                        parentId = 0;
                        var parent = await _organDepartmentRepository.GetAsync(request.ParentId);
                        departmentId = parent.DepartmentId;
                        companyId = parent.CompanyId;
                    }

                    var duty = new OrganDuty()
                    {
                        ParentId = parentId,
                        Name = request.Name,
                        CompanyId = companyId,
                        DepartmentId = departmentId,
                        CreatorId = admin.Id
                    };

                    var dutyId = await _organDutyRepository.InsertAsync(duty);
                    await _authManager.AddAdminLogAsync("新增岗位", request.Name);

                    await _authManager.AddStatLogAsync(StatType.DutyAdd, "新增岗位", dutyId, duty.Name);
                    await _authManager.AddStatCount(StatType.DutyAdd);
                }
            }
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
