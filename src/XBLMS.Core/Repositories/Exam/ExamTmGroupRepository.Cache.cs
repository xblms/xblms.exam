using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamTmGroupRepository
    {
        public async Task<ExamTmGroup> GetAsync(int id)
        {
            var list = await GetListAsync();
            return list.FirstOrDefault(group => group.Id == id);
        }

        public async Task<List<ExamTmGroup>> GetListAsync()
        {
            var list = (await _repository.GetAllAsync(Q
                .OrderByDesc(nameof(ExamTmGroup.Id))
                .CachingGet(CacheKey)
            )).ToList();

            return list;
        }
        public async Task<List<ExamTmGroup>> GetListWithoutLockedAsync()
        {
            var list = (await _repository.GetAllAsync(Q
                .WhereNullOrFalse(nameof(ExamTmGroup.Locked))
                .OrderByDesc(nameof(ExamTmGroup.Id))
                .CachingGet(CacheKey)
            )).ToList();

            return list.Where(g => g.Locked == false).ToList();
        }
    }
}
