using Datory;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperRandomTmRepository
    {
        private static readonly ConcurrentDictionary<string, Repository<ExamPaperRandomTm>> TableNameRepositories = new ConcurrentDictionary<string, Repository<ExamPaperRandomTm>>();
        private async Task<string> GetTableNameAsync(int examPaperId)
        {
            var examPaper = await _examPaperRepository.GetAsync(examPaperId);
            if (examPaper != null && examPaper.SeparateStorage)
            {
                return GetNewTableNameAsync(examPaperId);
            }
            return _repository.TableName;
        }
        private async Task<Repository<ExamPaperRandomTm>> GetRepositoryAsync(string tableName)
        {
            if (TableNameRepositories.TryGetValue(tableName, out var repository))
            {
                return repository;
            }

            repository = new Repository<ExamPaperRandomTm>(_settingsManager.Database, tableName, _settingsManager.Redis);
            await repository.LoadTableColumnsAsync(tableName);

            TableNameRepositories[tableName] = repository;
            return repository;
        }


        private string GetNewTableNameAsync(int examPaperId)
        {
            var tableName = $"{_repository.TableName}_{examPaperId}";
            return tableName;
        }
        public async Task CreateSeparateStorageAsync(int examPaperId)
        {
            var tableName = GetNewTableNameAsync(examPaperId);
            await CreateTableAsync(tableName, _repository.TableColumns);
        }

        private async Task CreateTableAsync(string tableName, List<TableColumn> columnInfoList)
        {
            var isDbExists = await _settingsManager.Database.IsTableExistsAsync(tableName);
            if (isDbExists) return;

            await _settingsManager.Database.CreateTableAsync(tableName, columnInfoList);

            await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperRandomTm.ExamPaperId)}", nameof(ExamPaperRandomTm.ExamPaperId));
            await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperRandomTm.SourceTmId)}", nameof(ExamPaperRandomTm.SourceTmId));
            await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperRandomTm.ExamPaperRandomId)}", nameof(ExamPaperRandomTm.ExamPaperRandomId));
        }
    }
}
