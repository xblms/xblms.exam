using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IBlockRuleRepository : IRepository
    {
        Task<BlockRule> GetAsync(int ruleId);

        Task<int> InsertAsync(BlockRule rule);

        Task<bool> UpdateAsync(BlockRule rule);

        Task<bool> IsExistsAsync(string ruleName);

        Task DeleteAsync(int ruleId);

        Task<List<BlockRule>> GetAllAsync(int type=1);
    }
}
