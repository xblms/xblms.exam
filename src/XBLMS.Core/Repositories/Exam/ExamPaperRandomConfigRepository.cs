using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperRandomConfigRepository : IExamPaperRandomConfigRepository
    {
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<ExamPaperRandomConfig> _repository;

        public ExamPaperRandomConfigRepository(ISettingsManager settingsManager, IExamPaperRepository examPaperRepository)
        {
            _settingsManager = settingsManager;
            _examPaperRepository = examPaperRepository;
            _repository = new Repository<ExamPaperRandomConfig>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<List<ExamPaperRandomConfig>> GetListAsync(int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            var infoList = await repository.GetAllAsync(
                Q.Where(nameof(ExamPaperRandomConfig.ExamPaperId), examPaperId).
                OrderBy(nameof(ExamPaperRandomConfig.TxTaxis)));
            return infoList;
        }
        public async Task<int> InsertAsync(ExamPaperRandomConfig item)
        {
            var tableName = await GetTableNameAsync(item.ExamPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.InsertAsync(item);
        }
        public async Task<int> DeleteByPaperAsync(int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.DeleteAsync(Q.Where(nameof(ExamPaperRandomConfig.ExamPaperId), examPaperId));
        }

    }
}
