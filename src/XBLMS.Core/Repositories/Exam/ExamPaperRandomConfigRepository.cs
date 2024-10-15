using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using XBLMS.Repositories;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamPaperRandomConfigRepository : IExamPaperRandomConfigRepository
    {
        private readonly Repository<ExamPaperRandomConfig> _repository;

        public ExamPaperRandomConfigRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPaperRandomConfig>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<List<ExamPaperRandomConfig>> GetListAsync(int examPaperId)
        {
            var infoList = await _repository.GetAllAsync(
                Q.Where(nameof(ExamPaperRandomConfig.ExamPaperId), examPaperId).
                OrderBy(nameof(ExamPaperRandomConfig.TxTaxis)));
            return infoList;
        }
        public async Task<int> InsertAsync(ExamPaperRandomConfig item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamPaperRandomConfig item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<int> DeleteByPaperAsync(int examPaperId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(ExamPaperRandomConfig.ExamPaperId), examPaperId));
        }

    }
}
