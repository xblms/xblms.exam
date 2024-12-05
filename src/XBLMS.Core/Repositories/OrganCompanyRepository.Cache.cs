using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class OrganCompanyRepository
    {
        public async Task<List<OrganCompany>> GetListAsync()
        {
            return await GetAllAsync();
        }
        public async Task<List<OrganCompany>> GetAllAsync()
        {
            return await _repository.GetAllAsync(Q
                .OrderBy(nameof(OrganCompany.CreatedDate))
                .CachingGet(_cacheKey)
            );
        }
        public async Task<OrganCompany> GetAsync(string name)
        {
            var compsnys = await GetAllAsync();
            var list = compsnys.ToList();
            return list.Where(x => x.Name == name).FirstOrDefault();
        }
        public async Task<OrganCompany> GetAsync(int id)
        {
            var compsnys = await GetAllAsync();
            var list = compsnys.ToList();
            return list.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<OrganCompany> GetByGuidAsync(string guid)
        {
            var compsnys = await GetAllAsync();
            var list = compsnys.ToList();
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
            var selectList = all.Where(c => ids.Contains(c.Id));

            return selectList.Select(c => c.Guid).ToList();
        }
        private async Task GetIdsAsync(List<int> ids, List<OrganCompany> all, int parentid)
        {
            var children = all.Where(x => x.ParentId == parentid).ToList();
            foreach (var child in children)
            {
                ids.Add(child.Id);
                await GetIdsAsync(ids, all, child.Id);
            }
        }


        public async Task<List<OrganCompany>> GetPathNamesAsync(int id)
        {
            var result = new List<OrganCompany>();
            var info = await GetAsync(id);
            if (info != null)
            {
                result.Add(info);
                await GetPathNamesAsync(result, info.ParentId);
            }
            result = result.OrderBy(d => d.Id).ToList();
            return result;
        }
        public async Task GetPathNamesAsync(List<OrganCompany> names, int parentId)
        {
            if (parentId > 0)
            {
                var info = await GetAsync(parentId);
                names.Add(info);
                await GetPathNamesAsync(names, info.ParentId);
            }
        }


        public async Task<(int allCount, int addCount,int deleteCount,int lockedCount,int unLockedCount)> GetDataCount()
        {
            var count = await _repository.CountAsync();
            return (count, 0, 0, 0, count);
        }
    }
}
