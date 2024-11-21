using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class ExamTmTreeRepository
    {
        public async Task<List<ExamTmTree>> GetListAsync()
        {
            return await GetAllAsync();
        }
        public async Task<List<ExamTmTree>> GetAllAsync()
        {
            return await _repository.GetAllAsync(Q
                .OrderBy(nameof(ExamTmTree.CreatedDate))
                .CachingGet(_cacheKey)
            );
        }

        public async Task<ExamTmTree> GetAsync(int id)
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

        private async Task GetIdsAsync(List<int> ids, List<ExamTmTree> all, int parentid)
        {
            var children = all.Where(x => x.ParentId == parentid).ToList();
            foreach (var child in children)
            {
                ids.Add(child.Id);
                await GetIdsAsync(ids, all, child.Id);
            }
        }


        public async Task<string> GetPathNamesAsync(int id)
        {
            var result = new List<ExamTmTree>();
            var info = await GetAsync(id);
            if (info != null)
            {
                result.Add(info);
                await GetPathNamesAsync(result, info.ParentId);
            }
            result = result.OrderBy(d => d.Id).ToList();
            var names = new List<string>();
            if(result.Count > 0)
            {
                foreach (var item in result)
                {
                    names.Add(item.Name);
                }

            }
            return ListUtils.ToString(names, ">"); ;
        }
        public async Task GetPathNamesAsync(List<ExamTmTree> names, int parentId)
        {
            if (parentId > 0)
            {
                var info = await GetAsync(parentId);
                names.Add(info);
                await GetPathNamesAsync(names, info.ParentId);
            }
        }

    }
}
