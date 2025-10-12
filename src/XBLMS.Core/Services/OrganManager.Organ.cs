using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class OrganManager
    {
        public async Task<string> GetOrganName(int departmentId, int companyId)
        {
            var names = new List<string>();

            if (departmentId > 0)
            {
                var department = await _departmentRepository.GetAsync(departmentId);
                if (department.ParentNames != null)
                {
                    names.AddRange(department.ParentNames);
                }
            }
            else
            {
                if (companyId > 0)
                {
                    var company = await _companyRepository.GetAsync(companyId);
                    if (company.ParentNames != null)
                    {
                        names.AddRange(company.ParentNames);
                    }
                    else
                    {
                        names.Add(company.Name);
                    }
                }
            }

            return ListUtils.ToString(names, "/");
        }

        public async Task<List<OrganTree>> GetOrganTreeTableDataLazyByChangeAsync(AdminAuth auth, int parentId, string keyWords)
        {
            var list = new List<OrganTree>();

            var parentGuid = string.Empty;
            var allCompanys = new List<OrganCompany>();
            if (!string.IsNullOrEmpty(keyWords))
            {
                allCompanys = await _companyRepository.GetListAsync(auth, keyWords);
            }
            else
            {
                if (parentId > 0)
                {
                    var parent = await _companyRepository.GetAsync(parentId);
                    parentGuid = parent.Guid;
                    allCompanys = await _companyRepository.GetListAsync(parentId);
                }
                else
                {
                    var tomCompany = await _companyRepository.GetAsync(auth.CompanyId);
                    allCompanys.Add(tomCompany);
                }
            }

            foreach (var company in allCompanys)
            {
                var adminTotal = 0;
                var adminCount = 0;
                var userTotal = 0;
                var userCount = 0;

                var fullName = company.Name;
                if (!string.IsNullOrEmpty(keyWords))
                {
                    var parentNames = string.IsNullOrEmpty(ListUtils.ToString(company.ParentNames, "/")) ? company.Name : ListUtils.ToString(company.ParentNames, "/");
                    fullName = StringUtils.ReplaceIgnoreCase(parentNames, keyWords, $"<span style='color:red;'>{keyWords}</span>");
                }

                list.Add(new OrganTree
                {
                    Id = company.Id,
                    Guid = company.Guid,
                    ParentGuid = parentGuid,
                    Name = company.Name,
                    FullName = fullName,
                    OrganType = "company",
                    OrganTypeName = "单位",
                    ParentId = company.ParentId,
                    AdminCount = adminCount,
                    AdminAllCount = adminTotal,
                    UserCount = userCount,
                    UserAllCount = userTotal,
                    LastModifiedDate = company.LastModifiedDate,
                    HasChildren = string.IsNullOrEmpty(keyWords) && await _companyRepository.HasChildren(company.Id),

                });
            }

            return list;
        }

        public async Task<List<OrganTree>> GetOrganTreeTableDataLazySearchAsync(AdminAuth auth, string keyWords, bool showAdminTotal = true, bool showUserTotal = true)
        {
            var list = new List<OrganTree>();
            var allCompanys = await _companyRepository.GetListAsync(auth, keyWords);
            foreach (var company in allCompanys)
            {
                var adminTotal = 0;
                var adminCount = 0;
                var userTotal = 0;
                var userCount = 0;

                if (showAdminTotal)
                {
                    (adminTotal, adminCount) = await _administratorRepository.GetCountByCompanyAsync(auth, company.Id);
                }
                if (showUserTotal)
                {
                    (userTotal, userCount) = await _userRepository.GetCountByCompanyAsync(auth, company.Id);
                }

                var parentNames = string.IsNullOrEmpty(ListUtils.ToString(company.ParentNames, "/")) ? company.Name : ListUtils.ToString(company.ParentNames, "/");
                var fullName = StringUtils.ReplaceIgnoreCase(parentNames, keyWords, $"<span style='color:red;'>{keyWords}</span>");
                list.Add(new OrganTree
                {
                    Id = company.Id,
                    Guid = company.Guid,
                    Name = company.Name,
                    FullName = fullName,
                    OrganType = "company",
                    OrganTypeName = "单位",
                    ParentId = company.ParentId,
                    AdminCount = adminCount,
                    AdminAllCount = adminTotal,
                    UserCount = userCount,
                    UserAllCount = userTotal,
                    LastModifiedDate = company.LastModifiedDate,
                    HasChildren = false,
                    IsLeaf = true
                });
            }

            var allDepartments = await _departmentRepository.GetListAsync(auth, keyWords);
            foreach (var department in allDepartments)
            {
                var adminTotal = 0;
                var adminCount = 0;
                var userTotal = 0;
                var userCount = 0;

                if (showAdminTotal)
                {
                    (adminTotal, adminCount) = await _administratorRepository.GetCountByDepartmentAsync(auth, department.Id);
                }
                if (showUserTotal)
                {
                    (userTotal, userCount) = await _userRepository.GetCountByDepartmentAsync(auth, department.Id);
                }

                var parentNames = string.IsNullOrEmpty(ListUtils.ToString(department.ParentNames, "/")) ? department.Name : ListUtils.ToString(department.ParentNames, "/");
                var fullName = StringUtils.ReplaceIgnoreCase(parentNames, keyWords, $"<span style='color:red;'>{keyWords}</span>");
                list.Add(new OrganTree
                {
                    Id = department.Id,
                    Guid = department.Guid,
                    Name = department.Name,
                    FullName = fullName,
                    OrganType = "department",
                    OrganTypeName = "部门",
                    ParentId = department.ParentId,
                    AdminCount = adminCount,
                    AdminAllCount = adminTotal,
                    UserCount = userCount,
                    UserAllCount = userTotal,
                    LastModifiedDate = department.LastModifiedDate,
                    HasChildren = false,
                    IsLeaf = true
                });
            }
            return list;
        }
        public async Task<List<OrganTree>> GetOrganTreeTableDataLazyAsync(AdminAuth auth, int parentId, string organType, bool showAdminTotal = true, bool showUserTotal = true)
        {
            var list = new List<OrganTree>();
            if (organType == "company")
            {
                var allCompanys = new List<OrganCompany>();
                var parentGuid = string.Empty;
                if (parentId > 0)
                {
                    var parent = await _companyRepository.GetAsync(parentId);
                    parentGuid = parent.Guid;
                    allCompanys = await _companyRepository.GetListAsync(parentId);
                }
                else
                {
                    var tomCompany = await _companyRepository.GetAsync(auth.CompanyId);
                    allCompanys.Add(tomCompany);
                }


                foreach (var company in allCompanys)
                {
                    var adminTotal = 0;
                    var adminCount = 0;
                    var userTotal = 0;
                    var userCount = 0;
                    if (showAdminTotal)
                    {
                        (adminTotal, adminCount) = await _administratorRepository.GetCountByCompanyAsync(auth, company.Id);
                    }
                    if (showUserTotal)
                    {
                        (userTotal, userCount) = await _userRepository.GetCountByCompanyAsync(auth, company.Id);
                    }

                    list.Add(new OrganTree
                    {
                        Id = company.Id,
                        Guid = company.Guid,
                        ParentGuid = parentGuid,
                        Name = company.Name,
                        FullName = company.Name,
                        OrganType = "company",
                        OrganTypeName = "单位",
                        ParentId = company.ParentId,
                        AdminCount = adminCount,
                        AdminAllCount = adminTotal,
                        UserCount = userCount,
                        UserAllCount = userTotal,
                        LastModifiedDate = company.LastModifiedDate,
                        HasChildren = await _companyRepository.HasChildren(company.Id) || await _departmentRepository.HasChildren(0, company.Id),

                    });
                }
                var allDepartments = await _departmentRepository.GetListAsync(0, parentId);
                foreach (var department in allDepartments)
                {
                    var adminTotal = 0;
                    var adminCount = 0;
                    var userTotal = 0;
                    var userCount = 0;
                    if (showAdminTotal)
                    {
                        (adminTotal, adminCount) = await _administratorRepository.GetCountByDepartmentAsync(auth, department.Id);
                    }
                    if (showUserTotal)
                    {
                        (userTotal, userCount) = await _userRepository.GetCountByDepartmentAsync(auth, department.Id);
                    }

                    list.Add(new OrganTree
                    {
                        Id = department.Id,
                        Guid = department.Guid,
                        ParentGuid = parentGuid,
                        Name = department.Name,
                        FullName = department.Name,
                        OrganType = "department",
                        OrganTypeName = "部门",
                        ParentId = department.ParentId,
                        AdminCount = adminCount,
                        AdminAllCount = adminTotal,
                        UserCount = userCount,
                        UserAllCount = userTotal,
                        LastModifiedDate = department.LastModifiedDate,
                        HasChildren = await _departmentRepository.HasChildren(department.Id, department.CompanyId),
                    });
                }
            }
            else
            {
                var allDepartments = new List<OrganDepartment>();
                var parentGuid = string.Empty;

                if (parentId > 0)
                {
                    var dinfo = await _departmentRepository.GetAsync(parentId);
                    if (dinfo != null)
                    {
                        parentGuid = dinfo.Guid;
                    }
                    allDepartments = await _departmentRepository.GetListAsync(dinfo.Id, dinfo.CompanyId);

                }


                foreach (var department in allDepartments)
                {
                    var adminTotal = 0;
                    var adminCount = 0;
                    var userTotal = 0;
                    var userCount = 0;
                    if (showAdminTotal)
                    {
                        (adminTotal, adminCount) = await _administratorRepository.GetCountByDepartmentAsync(auth, department.Id);
                    }
                    if (showUserTotal)
                    {
                        (userTotal, userCount) = await _userRepository.GetCountByDepartmentAsync(auth, department.Id);
                    }

                    list.Add(new OrganTree
                    {
                        Id = department.Id,
                        Guid = department.Guid,
                        ParentGuid = parentGuid,
                        Name = department.Name,
                        FullName = department.Name,
                        OrganType = "department",
                        OrganTypeName = "部门",
                        ParentId = department.ParentId,
                        AdminCount = adminCount,
                        AdminAllCount = adminTotal,
                        UserCount = userCount,
                        UserAllCount = userTotal,
                        LastModifiedDate = department.LastModifiedDate,
                        HasChildren = await _departmentRepository.HasChildren(department.Id, department.CompanyId),
                    });
                }
            }

            return list;
        }

    }
}
