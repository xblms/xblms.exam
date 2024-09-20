using CacheManager.Core;
using Datory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Core.Services;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Database
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class BackupController : ControllerBase
    {
        private const string RouteBackup = "settings/database/backup";
        private const string RouteBackupExcution = "settings/database/backup/excution";
        private const string RouteBackupDelete = "settings/database/backup/delbackup";

        private const string RouteBackupRecover = "settings/database/backup/recover";
        private const string RouteBackupRecoverlog = "settings/database/backup/recoverlog";
        private const string RouteBackupRecoverlogDel = "settings/database/backup/recoverlogdel";


        private readonly IDatabaseManager _databaseManager;
        private readonly IAuthManager _authManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IDbBackupRepository _dbBackupRepository;
        private readonly IDbRecoverRepository _dbRecoverRepository;
        public BackupController(IDatabaseManager databaseManager, IAuthManager authManager, ISettingsManager settingsManager, IConfigRepository configRepository,
             IDbBackupRepository dbBackupRepository, IDbRecoverRepository dbRecoverRepository)
        {
            _databaseManager = databaseManager;
            _authManager = authManager;
            _settingsManager = settingsManager;
            _dbBackupRepository = dbBackupRepository;
            _dbRecoverRepository = dbRecoverRepository;
        }

        public class TableInfo
        {
            public List<TableColumn> Columns { get; set; }
            public int TotalCount { get; set; }
            public List<string> RowFiles { get; set; }
        }
        public class GetRequest
        {
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
            public string Token { get; set; }
            public bool IsFileServer { get; set; }
        }
        public class GetResult
        {
            public int Total { get; set; }
            public List<DbBackup> List { get; set; }
        }
        public class GetRecoverResult
        {
            public int Total { get; set; }
            public List<DbRecover> List { get; set; }

        }

        private async Task<string> Backup()
        {
            var beginTime = DateTime.Now;
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
                    var columns = await _settingsManager.Database.GetTableColumnsAsync(tableName);
                    var repository = new Repository(_settingsManager.Database, tableName, columns);

                    var tableInfo = new TableInfo
                    {
                        Columns = repository.TableColumns,
                        TotalCount = await repository.CountAsync(),
                        RowFiles = new List<string>()
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
                    successTables.Add(tableName);
                }
                catch (Exception ex)
                {
                    errorTables.Add(tableName);
                    errorLog += $"{tableName}:{ex};";
                }
            }
            var jobinfo = new DbBackup();
            jobinfo.BeginTime = beginTime;
            jobinfo.EndTime = DateTime.Now;
            jobinfo.Status = errorTables.Count > 0 ? 2 : 1;
            jobinfo.ErrorLog = errorLog.ToString();
            jobinfo.SuccessTables = successTables;
            jobinfo.ErrorTables = errorTables;
            jobinfo.FilePath = directory;
            jobinfo.DataSize = FileUtils.GetFileSizeByFileLength(dataSize);
            await _dbBackupRepository.InsertAsync(jobinfo);

            return jobinfo.EndTime.ToString();
        }
    }
}
