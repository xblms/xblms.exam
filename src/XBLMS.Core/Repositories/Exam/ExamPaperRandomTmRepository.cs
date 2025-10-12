using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperRandomTmRepository : IExamPaperRandomTmRepository
    {
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<ExamPaperRandomTm> _repository;

        public ExamPaperRandomTmRepository(ISettingsManager settingsManager, IExamPaperRepository examPaperRepository)
        {
            _settingsManager = settingsManager;
            _examPaperRepository = examPaperRepository;
            _repository = new Repository<ExamPaperRandomTm>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<ExamPaperRandomTm> GetAsync(int Id,int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.GetAsync(Id);
        }


        public async Task<int> InsertAsync(ExamPaperRandomTm item)
        {
            var tableName = await GetTableNameAsync(item.ExamPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.InsertAsync(item);
        }

        public async Task<int> DeleteByPaperAsync(int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.DeleteAsync(Q.Where(nameof(ExamPaperRandomTm.ExamPaperId), examPaperId));
        }

        public async Task<int> DeleteByRandomIdAsync(int examPaperRandomId, int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.DeleteAsync(Q.Where(nameof(ExamPaperRandomTm.ExamPaperRandomId), examPaperRandomId));
        }
        public async Task<List<ExamPaperRandomTm>> GetListAsync(int examPaperRandomId, int txId, int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            var infoList = await repository.GetAllAsync(Q.
                Where(nameof(ExamPaperRandomTm.ExamPaperRandomId), examPaperRandomId).
                Where(nameof(ExamPaperRandomTm.TxId), txId).
                OrderBy(nameof(ExamPaperRandomTm.Id)));
            return infoList;
        }
        public async Task<List<int>> GetIdsAsync(int tmId,int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            var idsList = await repository.GetAllAsync<int>(Q.
                Select(nameof(ExamPaperRandomTm.Id)).
                Where(nameof(ExamPaperRandomTm.SourceTmId), tmId));
            return idsList;
        }
    }
}
