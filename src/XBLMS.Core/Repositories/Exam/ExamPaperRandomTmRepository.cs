using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamPaperRandomTmRepository : IExamPaperRandomTmRepository
    {
        private readonly Repository<ExamPaperRandomTm> _repository;

        public ExamPaperRandomTmRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPaperRandomTm>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<ExamPaperRandomTm> GetAsync(int Id)
        {
            return await _repository.GetAsync(Id);
        }


        public async Task<int> InsertAsync(ExamPaperRandomTm item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<int> DeleteByPaperAsync(int examPaperId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(ExamPaperRandomTm.ExamPaperId), examPaperId));
        }

        public async Task<List<ExamPaperRandomTm>> GetListAsync(int examPaperRandomId)
        {
            var infoList = await _repository.GetAllAsync(Q.
                Where(nameof(ExamPaperRandomTm.ExamPaperRandomId), examPaperRandomId).
                OrderBy(nameof(ExamPaperRandomTm.Id)));
            return infoList;
        }


        public async Task<List<ExamPaperRandomTm>> GetListAsync(int examPaperRandomId, int txId)
        {
            var infoList = await _repository.GetAllAsync(Q.
                Where(nameof(ExamPaperRandomTm.ExamPaperRandomId), examPaperRandomId).
                Where(nameof(ExamPaperRandomTm.TxId), txId).
                OrderBy(nameof(ExamPaperRandomTm.Id)));
            return infoList;
        }
        public async Task<List<int>> GetTmIdsAsync(int examPaperRandomId, int txId)
        {
            var infoList = await _repository.GetAllAsync<int>(Q.
                Select(nameof(ExamPaperRandomTm.Id)).
                Where(nameof(ExamPaperRandomTm.ExamPaperRandomId), examPaperRandomId).
                Where(nameof(ExamPaperRandomTm.TxId), txId).
                OrderBy(nameof(ExamPaperRandomTm.Id)));
            return infoList;
        }
        public async Task UpdateScoreAsync(ExamPaperRandomTm item)
        {
            await _repository.UpdateAsync(item);
        }
    }
}
