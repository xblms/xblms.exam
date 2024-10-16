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
            var query = Q.OrderByDesc(nameof(ExamTmGroup.Id));

            var list = await _repository.GetAllAsync(query);
            if (list != null && list.Count > 0)
            {
                return list;
            }
            else
            {
                await ResetAsync();
                list = await _repository.GetAllAsync(query);
            }
            return list;
        }
        public async Task<List<ExamTmGroup>> GetListWithoutLockedAsync()
        {
            var list = await GetListAsync();
            return list.Where(g => g.Locked == false).ToList();
        }
    }
}
