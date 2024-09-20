using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XBLMS.Repositories
{
    public interface IBlockAnalysisRepository : IRepository
    {
        Task AddBlockAsync(int blockType=1);

        Task<List<KeyValuePair<string, int>>> GetMonthlyBlockedListAsync(int blockType = 1);
    }
}
