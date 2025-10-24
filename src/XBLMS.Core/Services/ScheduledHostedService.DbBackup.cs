using Datory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class ScheduledHostedService
    {
        private async Task DbBackupAsync()
        {
            var config = await _databaseManager.ConfigRepository.GetAsync();
            var isBackuping = await _databaseManager.DbBackupRepository.ExistsBackupingAsync();
            if (isBackuping)
            {
                await ExecuteBackupAsync();
            }
            else
            {
                var existsToday = await _databaseManager.DbBackupRepository.ExistsTodayAsync();
                if (!existsToday && config.DbBackupAuto)
                {
                    await _databaseManager.DbBackupRepository.InsertAsync(new DbBackup
                    {
                        BeginTime = DateTime.Now,
                        Status = 0
                    });
                    await ExecuteBackupAsync();
                }
            }
        }

        private async Task ExecuteBackupAsync()
        {
            var backupinfo = await _databaseManager.DbBackupRepository.GetBackupingAsync();

            var directory = $"sitefiles/dbbackup/{DateTime.Now:yyyy-MM-dd-hh-mm-ss}";

            var allTableNames = await _settingsManager.Database.GetTableNamesAsync();

            var tableNames = new List<string>();

            foreach (var tableName in allTableNames)
            {
                tableNames.Add(tableName);
            }
            var tablesFilePath = PathUtils.Combine(_settingsManager.WebRootPath, directory, "_tables.json");
            await FileUtils.WriteTextAsync(tablesFilePath, TranslateUtils.JsonSerialize(tableNames));

            var successTables = new List<string>();
            var errorTables = new List<string>();
            var errorLog = "";
            long dataSize = 0;
            foreach (var tableName in tableNames)
            {
                try
                {
                    dataSize += await BackupTable(tableName, directory);
                    successTables.Add(tableName);

                    if (_databaseManager.ExamPaperRepository.TableName.Equals(tableName))
                    {
                        var tableIdList = await _databaseManager.ExamPaperRepository.Select_GetSeparateStorageIdList();
                        if (tableIdList != null && tableIdList.Count > 0)
                        {
                            foreach (var tableId in tableIdList)
                            {
                                var ExamPaperAnswer_TableName = _databaseManager.ExamPaperAnswerRepository.GetNewTableNameAsync(tableId);
                                dataSize += await BackupTable(ExamPaperAnswer_TableName, directory);
                                var ExamPaperRandomConfig_TableName = _databaseManager.ExamPaperRandomConfigRepository.GetNewTableNameAsync(tableId);
                                dataSize += await BackupTable(ExamPaperRandomConfig_TableName, directory);
                                var ExamPaperRandom_TableName = _databaseManager.ExamPaperRandomRepository.GetNewTableNameAsync(tableId);
                                dataSize += await BackupTable(ExamPaperRandom_TableName, directory);
                                var ExamPaperRandomTm_TableName = _databaseManager.ExamPaperRandomTmRepository.GetNewTableNameAsync(tableId);
                                dataSize += await BackupTable(ExamPaperRandomTm_TableName, directory);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorTables.Add(tableName);
                    errorLog += $"{tableName}:{ex};";
                }
            }

            backupinfo.EndTime = DateTime.Now;
            backupinfo.Status = errorTables.Count > 0 ? 2 : 1;
            backupinfo.ErrorLog = errorLog.ToString();
            backupinfo.SuccessTables = successTables;
            backupinfo.ErrorTables = errorTables;
            backupinfo.FilePath = directory;
            backupinfo.DataSize = FileUtils.GetFileSizeByFileLength(dataSize);
            await _databaseManager.DbBackupRepository.UpdateAsync(backupinfo);

        }
        private async Task<long> BackupTable(string tableName, string directory)
        {
            long dataSize = 0;
            var columns = await _settingsManager.Database.GetTableColumnsAsync(tableName);
            var repository = new Repository(_settingsManager.Database, tableName, columns);

            var tableInfo = new TableInfo
            {
                Columns = repository.TableColumns,
                TotalCount = await repository.CountAsync(),
                RowFiles = []
            };

            if (tableInfo.TotalCount > 0)
            {
                var fileName = $"{tableName}.json";
                tableInfo.RowFiles.Add(fileName);
                var rows = await _databaseManager.GetObjectsAsync(tableName);

                var filepath = PathUtils.Combine(_settingsManager.WebRootPath, directory, fileName);
                await FileUtils.WriteTextAsync(filepath, TranslateUtils.JsonSerialize(rows));
                dataSize += FileUtils.GetFileSizeLongByFilePath(filepath);
            }
            var metapath = PathUtils.Combine(_settingsManager.WebRootPath, directory, tableName, "_metadata.json");
            await FileUtils.WriteTextAsync(metapath, TranslateUtils.JsonSerialize(tableInfo));
            return dataSize;
        }
        private class TableInfo
        {
            public List<TableColumn> Columns { get; set; }
            public int TotalCount { get; set; }
            public List<string> RowFiles { get; set; }
        }
    }
}
