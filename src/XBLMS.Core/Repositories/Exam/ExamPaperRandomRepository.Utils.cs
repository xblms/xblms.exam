﻿using Datory;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperRandomRepository
    {
        private static readonly ConcurrentDictionary<string, Repository<ExamPaperRandom>> TableNameRepositories = new ConcurrentDictionary<string, Repository<ExamPaperRandom>>();
        private async Task<string> GetTableNameAsync(int examPaperId)
        {
            var examPaper = await _examPaperRepository.GetAsync(examPaperId);
            if (examPaper != null && examPaper.SeparateStorage)
            {
                return GetNewTableNameAsync(examPaperId);
            }
            return _repository.TableName;
        }
        private async Task<Repository<ExamPaperRandom>> GetRepositoryAsync(string tableName)
        {
            if (TableNameRepositories.TryGetValue(tableName, out var repository))
            {
                return repository;
            }

            repository = new Repository<ExamPaperRandom>(_settingsManager.Database, tableName, _settingsManager.Redis);
            await repository.LoadTableColumnsAsync(tableName);

            TableNameRepositories[tableName] = repository;
            return repository;
        }


        public string GetNewTableNameAsync(int examPaperId)
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

            await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperRandom.ExamPaperId)}", nameof(ExamPaperRandom.ExamPaperId));
        }
    }
}
