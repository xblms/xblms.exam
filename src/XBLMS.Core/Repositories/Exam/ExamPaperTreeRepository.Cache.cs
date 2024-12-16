using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperTreeRepository
    {
        public async Task<List<ExamPaperTree>> GetListAsync()
        {
            return await GetAllAsync();
        }
        public async Task<List<ExamPaperTree>> GetAllAsync()
        {
            return await _repository.GetAllAsync(Q
                .OrderBy(nameof(ExamPaperTree.CreatedDate))
                .CachingGet(_cacheKey)
            );
        }

        public async Task<ExamPaperTree> GetAsync(int id)
        {
            var tks = await GetAllAsync();
            var list = tks.ToList();
            return list.Where(x => x.Id == id).FirstOrDefault();
        }


        public async Task<List<int>> GetIdsAsync(int id)
        {
            var all = await GetAllAsync();
            var ids = new List<int>();
            ids.Add(id);
            await GetIdsAsync(ids, all, id);
            return ids;
        }

        private async Task GetIdsAsync(List<int> ids, List<ExamPaperTree> all, int parentid)
        {
            var children = all.Where(x => x.ParentId == parentid).ToList();
            foreach (var child in children)
            {
                ids.Add(child.Id);
                await GetIdsAsync(ids, all, child.Id);
            }
        }

    }
}
