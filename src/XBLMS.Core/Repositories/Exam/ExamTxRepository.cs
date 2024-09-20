using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class ExamTxRepository : IExamTxRepository
    {
        private readonly Repository<ExamTx> _repository;
        private readonly string _cacheKey;

        public ExamTxRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamTx>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<bool> IsExistsAsync(string name)
        {
            var list = await GetListAsync();
            return list.Any(tx => tx.Name == name);
        }

        public async Task<int> InsertAsync(ExamTx examTx)
        {
            return await _repository.InsertAsync(examTx, Q.CachingRemove(_cacheKey));
        }

        public async Task UpdateAsync(ExamTx examTx)
        {
            await _repository.UpdateAsync(examTx, Q.CachingRemove(_cacheKey));
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id, Q.CachingRemove(_cacheKey));
        }

        public async Task ResetAsync()
        {
            var typs = ListUtils.GetEnums<ExamTxBase>();
            var taxis = 1;
            foreach (var item in typs)
            {
                await _repository.InsertAsync(new ExamTx
                {
                    Name=item.GetDisplayName(),
                    ExamTxBase = item,
                    Score=2,
                    Taxis=taxis
                },Q.CachingRemove(_cacheKey));
                taxis++;
            }
        }
    }
}
