using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class BlockRuleRepository : IBlockRuleRepository
    {
        private readonly Repository<BlockRule> _repository;

        public BlockRuleRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<BlockRule>(settingsManager.Database, settingsManager.Redis);
        }
        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private static string GetCacheKey()
        {
            return $"XBLMS.Block.Core";
        }

        public async Task<BlockRule> GetAsync(int ruleId)
        {
            var rules = await GetAllAsync(0);
            return rules.FirstOrDefault(x => x.Id == ruleId);
        }

        public async Task<int> InsertAsync(BlockRule ad)
        {
            return await _repository.InsertAsync(ad, Q.CachingRemove(GetCacheKey()));
        }

        public async Task<bool> UpdateAsync(BlockRule ad)
        {
            return await _repository.UpdateAsync(ad, Q.CachingRemove(GetCacheKey()));
        }

        public async Task<bool> IsExistsAsync(string ruleName)
        {
            var rules = await GetAllAsync(0);
            return rules.Exists(x => x.RuleName == ruleName);
        }

        public async Task DeleteAsync(int ruleId)
        {
            await _repository.DeleteAsync(ruleId, Q.CachingRemove(GetCacheKey()));
        }

        public async Task<List<BlockRule>> GetAllAsync(int blockType=1)
        {
            var list= await _repository.GetAllAsync(Q
                .OrderByDesc(nameof(BlockRule.Id))
                .CachingGet(GetCacheKey())
            );
            if (blockType > 0)
            {
                return list.Where(m => m.BlockType == blockType).ToList();
            }
            return list;
        }
    }
}
