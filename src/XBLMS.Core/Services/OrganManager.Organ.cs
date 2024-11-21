using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class OrganManager
    {
        public async Task<string> GetOrganName(int dutyId, int departmentId, int companyId)
        {
            var names = new List<string>();
            var dutys = new List<OrganDuty>();
            var ds = new List<OrganDepartment>();
            var cs = new List<OrganCompany>();
            if (dutyId > 0)
            {
                dutys = await _dutyRepository.GetPathNamesAsync(dutyId);
            }
            if (departmentId > 0)
            {
                ds = await _departmentRepository.GetPathNamesAsync(departmentId);
            }
            if (companyId > 0)
            {
                cs = await _companyRepository.GetPathNamesAsync(companyId);
            }
            if (cs.Count > 0)
            {
                foreach (var c in cs)
                {
                    names.Add(c.Name);
                }
            }
            if (ds.Count > 0)
            {
                foreach (var d in ds)
                {
                    names.Add(d.Name);
                }
            }
            if (dutys.Count > 0)
            {
                foreach (var d in dutys)
                {
                    names.Add(d.Name);
                }
            }
            return ListUtils.ToString(names, ">");
        }

        public async Task<List<OrganTree>> GetOrganTreeTableDataAsync()
        {
            var list = new List<OrganTree>();
            var allCompanys = await _companyRepository.GetListAsync();
            var allOrgan = new List<OrganTree>();
            foreach (var company in allCompanys)
            {
                var userCount = await _userRepository.GetCountAsync(company.Id, 0, 0);
                var adminCount = await _administratorRepository.GetCountAsync(company.Id, 0, 0);
                var cids = await GetCompanyIdsAsync(company.Id);
                var adminChildCount = await _administratorRepository.GetCountAsync(cids, null, null);
                var userChildCount = await _userRepository.GetCountAsync(cids, null, null);

                allOrgan.Add(new OrganTree
                {
                    Id = company.Id,
                    Guid = company.Guid,
                    Name = company.Name,
                    OrganType = "company",
                    OrganTypeName = "单位",
                    ParentId = company.ParentId,
                    AdminCount = adminCount,
                    AdminAllCount = adminChildCount,
                    UserCount = userCount,
                    UserAllCount = userChildCount,
                });
            }

            var topOrgans = allOrgan.Where(c => c.ParentId == 0).ToList();
            foreach (var cur in topOrgans)
            {
                await SetOrganChildren(allOrgan, cur);
                list.Add(cur);
            }

            return list;
        }
        private async Task SetOrganChildren(List<OrganTree> all, OrganTree parentOrgan)
        {
            var children = all.Where(c => c.ParentId == parentOrgan.Id).ToList();

            var departmentChildren = await SetDepartment(parentOrgan.Id);

            if (children.Count > 0 || departmentChildren.Count > 0)
            {
                var pchildren = new List<OrganTree>();
                pchildren.AddRange(children);
                pchildren.AddRange(departmentChildren);
                parentOrgan.Children = pchildren;
            }

            foreach (var child in children)
            {
                await SetOrganChildren(all, child);
            }
        }

        private async Task<List<OrganTree>> SetDepartment(int companyId)
        {
            var departments = await _departmentRepository.GetListAsync(companyId);

            var allOrgan = new List<OrganTree>();
            foreach (var dept in departments)
            {
                var userCount = await _userRepository.GetCountAsync(0, dept.Id, 0);
                var adminCount = await _administratorRepository.GetCountAsync(0, dept.Id, 0);
                var dids = await GetDepartmentIdsAsync(dept.Id);
                var adminChildCount = await _administratorRepository.GetCountAsync(null, dids, null);
                var userChildCount = await _userRepository.GetCountAsync(null, dids, null);

                allOrgan.Add(new OrganTree
                {
                    Id = dept.Id,
                    Guid = dept.Guid,
                    Name = dept.Name,
                    OrganType = "department",
                    OrganTypeName = "部门",
                    ParentId = dept.ParentId,
                    AdminCount = adminCount,
                    AdminAllCount = adminChildCount,
                    UserCount = userCount,
                    UserAllCount = userChildCount,
                });
            }
            var topDepartmentList = allOrgan.Where(p => p.ParentId == 0).ToList();

            var DepartmentTree = new List<OrganTree>();
            foreach (var item in topDepartmentList)
            {
                var children = item;
                await SetDepartmentChildren(allOrgan, children);
                DepartmentTree.Add(children);
            }
            return DepartmentTree;
        }

        private async Task SetDepartmentChildren(List<OrganTree> all, OrganTree parent)
        {
            var subitems = all.Where(c => c.ParentId == parent.Id).ToList();

            var dutyChildren = await SetDuty(parent.Id);

            if (subitems.Count > 0 || dutyChildren.Count > 0)
            {
                var children = new List<OrganTree>();
                children.AddRange(subitems);
                children.AddRange(dutyChildren);
                parent.Children = children;
            }

            foreach (var item in subitems)
            {
                await SetDepartmentChildren(all, item);
            }

        }

        private async Task<List<OrganTree>> SetDuty(int departmentId)
        {
            var dutys = await _dutyRepository.GetListAsync(departmentId);

            var allOrgan = new List<OrganTree>();
            foreach (var duty in dutys)
            {
                var userCount = await _userRepository.GetCountAsync(0, 0, duty.Id);
                var adminCount = await _administratorRepository.GetCountAsync(0, 0, duty.Id);
                var dutyids = await GetDutyIdsAsync(duty.Id);
                var adminChildCount = await _administratorRepository.GetCountAsync(null, null, dutyids);
                var userChildCount = await _userRepository.GetCountAsync(null, null, dutyids);

                allOrgan.Add(new OrganTree
                {
                    Id = duty.Id,
                    Guid = duty.Guid,
                    Name = duty.Name,
                    OrganType = "duty",
                    OrganTypeName = "岗位",
                    ParentId = duty.ParentId,
                    AdminCount = adminCount,
                    AdminAllCount = adminChildCount,
                    UserCount = userCount,
                    UserAllCount = userChildCount,
                });
            }
            var topDutyList = allOrgan.Where(p => p.ParentId == 0).ToList();

            var DutyTree = new List<OrganTree>();
            foreach (var item in topDutyList)
            {
                var children = item;
                SetDutyChildren(allOrgan, children);
                DutyTree.Add(children);
            }
            return DutyTree;
        }
        private static void SetDutyChildren(List<OrganTree> all, OrganTree parent)
        {
            var subitems = all.Where(c => c.ParentId == parent.Id).ToList();

            if (subitems.Count > 0)
            {
                parent.Children = new List<OrganTree>(subitems);
            }

            foreach (var item in subitems)
            {
                SetDutyChildren(all, item);
            }
        }
    }
}
