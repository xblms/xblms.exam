using Datory;
using DocumentFormat.OpenXml.Office2010.Excel;
using NPOI.POIFS.Properties;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class KnowlegesTreeRepository
    {
        public async Task<List<KnowledgesTree>> GetListAsync()
        {
            return await GetAllAsync();
        }
        public async Task<List<KnowledgesTree>> GetAllAsync()
        {
            return await _repository.GetAllAsync(Q
                .OrderBy(nameof(KnowledgesTree.CreatedDate))
                .CachingGet(_cacheKey)
            );
        }

        public async Task<KnowledgesTree> GetAsync(int id)
        {
            var tks = await GetAllAsync();
            var list = tks.ToList();
            return list.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<List<string>> GetParentPathAsync(int id)
        {
            var current = await _repository.GetAsync(id);
            var ids = new List<int> { id };

            if (current.ParentId > 0)
            {
                await GetParentIdsAsync(ids, current.ParentId);
            }

            var path = new List<string>();
            for (var i = ids.Count - 1; i >= 0; i--)
            {
                path.Add($"'{ids[i]}'");
            }

            return path;
        }

        public async Task<List<int>> GetIdsAsync(int id)
        {
            var all = await GetAllAsync();
            var ids = new List<int>(id);
            await GetIdsAsync(ids, all, id);
            return ids;
        }

        private async Task GetIdsAsync(List<int> ids, List<KnowledgesTree> all, int parentid)
        {
            var children = all.Where(x => x.ParentId == parentid).ToList();
            foreach (var child in children)
            {
                ids.Add(child.Id);
                await GetIdsAsync(ids, all, child.Id);
            }
        }
        private async Task GetParentIdsAsync(List<int> ids, int parentId)
        {
            ids.Add(parentId);

            var current = await _repository.GetAsync(parentId);

            if (current.ParentId > 0)
            {
                await GetParentIdsAsync(ids, current.ParentId);
            }
        }

        public async Task<List<KnowledgesTree>> GetChildAsync(int parentId)
        {
            var all = await GetAllAsync();
            var children = all.Where(x => x.ParentId == parentId).ToList();
            return children;
        }
    }
}
