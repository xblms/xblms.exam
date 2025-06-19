using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamTxRepository
    {

        public async Task<ExamTx> GetAsync(int id)
        {
            var list = await GetListAsync();
            return list.FirstOrDefault(tx => tx.Id == id) ?? null;
        }
        public async Task<ExamTx> GetAsync(string txName)
        {
            var list = await GetListAsync();
            return list.FirstOrDefault(tx => tx.Name == txName) ?? null;
        }

        public async Task<List<ExamTx>> GetListAsync()
        {
            var list = (await _repository.GetAllAsync(Q
                .OrderBy(nameof(ExamTx.Taxis))
                .CachingGet(_cacheKey)
            )).ToList();
            return list;
        }
    }
}
