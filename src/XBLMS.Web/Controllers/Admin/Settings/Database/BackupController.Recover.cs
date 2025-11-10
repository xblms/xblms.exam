using Datory;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Database
{
    public partial class BackupController
    {
        [HttpPost, Route(RouteBackupRecoverlog)]
        public async Task<ActionResult<GetRecoverResult>> GetRecoverLog([FromBody] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var (total, list) = await _dbRecoverRepository.GetListAsync(request.PageIndex, request.PageSize);
            return new GetRecoverResult
            {
                Total = total,
                List = list
            };
        }
        [HttpPost, Route(RouteBackupRecoverlogDel)]
        public async Task<ActionResult<BoolResult>> DeleteRecoverLog()
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            await _dbRecoverRepository.DeleteAsync();
            await _authManager.AddAdminLogAsync("清空数据库还原日志");
            return new BoolResult { Value = true };
        }

        [HttpPost, Route(RouteBackupRecover)]
        public async Task<ActionResult<BoolResult>> Recover([FromBody] GetRecoverRequest request)
        {
            if (_settingsManager.SecurityKey != request.SecurityKey)
            {
                return this.Error("SecurityKey 输入错误！");
            }

            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
            {
                return this.NoAuth();
            }
            var job = await _dbBackupRepository.GetAsync(request.Id);
            var jobRecover = new DbRecover
            {
                JobId = job.Id,
                BeginTime = DateTime.Now,
            };
            if (job == null || job.Status == 0 || job.Status == 2 || string.IsNullOrWhiteSpace(job.FilePath))
            {
                jobRecover.EndTime = DateTime.Now;
                jobRecover.Status = 2;
                jobRecover.ErrorLog = "备份异常，无法恢复。";
                await _dbRecoverRepository.InsertAsync(jobRecover);
                return new BoolResult
                {
                    Value = false
                };

            }
            else
            {
                var directionryPath = PathUtils.Combine(_settingsManager.WebRootPath, job.FilePath);
                if (!DirectoryUtils.IsDirectoryExists(directionryPath))
                {
                    jobRecover.EndTime = DateTime.Now;
                    jobRecover.Status = 2;
                    jobRecover.ErrorLog = "恢复文件路径不存在。";
                    await _dbRecoverRepository.InsertAsync(jobRecover);
                    return new BoolResult
                    {
                        Value = false
                    };
                }
                var tablesFilePath = PathUtils.Combine(directionryPath, "_tables.json");
                if (!FileUtils.IsFileExists(tablesFilePath))
                {
                    jobRecover.EndTime = DateTime.Now;
                    jobRecover.Status = 2;
                    jobRecover.ErrorLog = "恢复数据结构不存在。";
                    await _dbRecoverRepository.InsertAsync(jobRecover);
                    return new BoolResult
                    {
                        Value = false
                    };
                }

                var tableNames = TranslateUtils.JsonDeserialize<List<string>>(await FileUtils.ReadTextAsync(tablesFilePath, Encoding.UTF8));
                var errorTableNames = new List<string>();
                var errorLogs = "";
                foreach (var tableName in tableNames)
                {
                    var includes = new List<string>
                    {
                        _dbRecoverRepository.TableName,
                        _dbBackupRepository.TableName
                    };
                    if (ListUtils.ContainsIgnoreCase(includes, tableName)) continue;
                    try
                    {
                        await RecoverTable(tableName, directionryPath, errorTableNames);

                        if (_databaseManager.ExamPaperRepository.TableName.Equals(tableName))
                        {
                            var tableIdList = await _databaseManager.ExamPaperRepository.Select_GetSeparateStorageIdList();
                            if (tableIdList != null && tableIdList.Count > 0)
                            {
                                foreach (var tableId in tableIdList)
                                {
                                    var ExamPaperAnswer_TableName = _databaseManager.ExamPaperAnswerRepository.GetNewTableNameAsync(tableId);
                                    await RecoverTable(ExamPaperAnswer_TableName, directionryPath, errorTableNames);
                                    var ExamPaperRandomConfig_TableName = _databaseManager.ExamPaperRandomConfigRepository.GetNewTableNameAsync(tableId);
                                    await RecoverTable(ExamPaperRandomConfig_TableName, directionryPath, errorTableNames);
                                    var ExamPaperRandom_TableName = _databaseManager.ExamPaperRandomRepository.GetNewTableNameAsync(tableId);
                                    await RecoverTable(ExamPaperRandom_TableName, directionryPath, errorTableNames);
                                    var ExamPaperRandomTm_TableName = _databaseManager.ExamPaperRandomTmRepository.GetNewTableNameAsync(tableId);
                                    await RecoverTable(ExamPaperRandomTm_TableName, directionryPath, errorTableNames);
                                }
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        errorLogs += $"{tableName}:{ex.ToString()};";
                    }
                }

                jobRecover.EndTime = DateTime.Now;
                jobRecover.ErrorTables = errorTableNames;
                jobRecover.ErrorLog = errorLogs;
                jobRecover.Status = 1;
                if (!string.IsNullOrEmpty(errorLogs) || errorTableNames.Count > 0)
                {
                    jobRecover.Status = 2;
                }
                await _authManager.AddAdminLogAsync("恢复数据库", $"备份任务id：{job.Id}");
                await _dbRecoverRepository.InsertAsync(jobRecover);
                return new BoolResult
                {
                    Value = jobRecover.Status != 2
                };
            }

        }
        private async Task RecoverTable(string tableName, string directionryPath, List<string> errorTableNames)
        {
            var metadataFilePath = PathUtils.Combine(_settingsManager.WebRootPath, directionryPath, tableName, "_metadata.json");

            if (FileUtils.IsFileExists(metadataFilePath))
            {

                var tableInfo = TranslateUtils.JsonDeserialize<TableInfo>(await FileUtils.ReadTextAsync(metadataFilePath, Encoding.UTF8));

                if (await _settingsManager.Database.IsTableExistsAsync(tableName))
                {
                    await _settingsManager.Database.DropTableAsync(tableName);
                }

                await _settingsManager.Database.CreateTableAsync(tableName, tableInfo.Columns);

                if (tableInfo.RowFiles.Count > 0)
                {
                    for (var i = 0; i < tableInfo.RowFiles.Count; i++)
                    {
                        var fileName = tableInfo.RowFiles[i];
                        var filepath = PathUtils.Combine(_settingsManager.WebRootPath, directionryPath, fileName);
                        var objects = TranslateUtils.JsonDeserialize<List<JObject>>(
                            await FileUtils.ReadTextAsync(filepath, Encoding.UTF8));

                        try
                        {
                            var repository = new Repository(_settingsManager.Database, tableName,
                                tableInfo.Columns);
                            await repository.BulkInsertAsync(objects);
                        }
                        catch
                        {
                            errorTableNames.Add(tableName);
                        }
                    }
                }
            }
        }
    }
}
