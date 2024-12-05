using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class OrganDutyRepository
    {
        public async Task<List<OrganDuty>> GetListAsync(int departmentId)
        {
            var list = await GetAllAsync();
            return list.Where(d => d.DepartmentId == departmentId).ToList();
        }
        public async Task<List<OrganDuty>> GetAllAsync()
        {
            return await _repository.GetAllAsync(Q
                .OrderBy(nameof(OrganDuty.CreatedDate))
                .CachingGet(_cacheKey)
            );
        }
        public async Task<OrganDuty> GetAsync(int companyId, int departmentId, string name)
        {
            var dutys = await GetAllAsync();
            var list = dutys.ToList();
            return list.Where(x => x.Name == name && x.CompanyId == companyId && x.DepartmentId == departmentId).FirstOrDefault();
        }
        public async Task<OrganDuty> GetAsync(int id)
        {
            var dutys = await GetAllAsync();
            var list = dutys.ToList();
            return list.Where(x => x.Id == id).FirstOrDefault();
        }
        public async Task<OrganDuty> GetByGuidAsync(string guid)
        {
            var dutys = await GetAllAsync();
            var list = dutys.ToList();
            return list.Where(x => x.Guid == guid).FirstOrDefault();
        }
        public async Task<List<int>> GetIdsAsync(int id)
        {
            var all = await GetAllAsync();
            var ids = new List<int>();
            ids.Add(id);
            await GetIdsAsync(ids, all, id);
            return ids;
        }
        public async Task<List<string>> GetGuidsAsync(List<int> ids)
        {
            var all = await GetAllAsync();
            var selectList = all.Where(d => ids.Contains(d.Id));

            return selectList.Select(d => d.Guid).ToList();
        }
        private async Task GetIdsAsync(List<int> ids,List<OrganDuty> all, int parentid)
        {
            var children = all.Where(x => x.ParentId == parentid).ToList();
            foreach (var child in children)
            {
                ids.Add(child.Id);
                await GetIdsAsync(ids, all, child.Id);
            }
        }
        public async Task<List<int>> GetIdsByDepartmentIdAsync(int departmentId)
        {
            var ids = new List<int>();

            var dutys = await GetAllAsync();
            var all = dutys.ToList();
            var children = all.Where(x => x.DepartmentId == departmentId);
            foreach (var child in children)
            {
                ids.Add(child.Id);
            }
            return ids;
        }

        public async Task<List<OrganDuty>> GetPathNamesAsync(int id)
        {
            var result= new List<OrganDuty>();
            var info = await GetAsync(id);
            if (info != null)
            {
                result.Add(info);
                await GetPathNamesAsync(result, info.ParentId);
            }
            result= result.OrderBy(d => d.Id).ToList();
            return result;
        }
        public async Task GetPathNamesAsync(List<OrganDuty> names,int parentId)
        {
            if (parentId > 0)
            {
                var info=await GetAsync(parentId);
                names.Add(info);
                await GetPathNamesAsync(names, info.ParentId);
            }
        }
        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount()
        {
            var count = await _repository.CountAsync();
            return (count, 0, 0, 0, count);
        }
    }
}
