using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Database
{
    public partial class BackupController
    {
        [HttpGet, Route(RouteBackupDownload)]
        public async Task<ActionResult<StringResult>> Download([FromQuery] GetDownloadRequest request)
        {
            var admin = await _authManager.GetAdminAsync();

            if (_settingsManager.SecurityKey != request.SecurityKey)
            {
                return this.Error("SecurityKey 输入错误！");
            }

            var job = await _dbBackupRepository.GetAsync(request.Id);
            if (job != null)
            {
                if (!string.IsNullOrWhiteSpace(job.FilePath))
                {
                    var directionryPath = PathUtils.Combine(_settingsManager.WebRootPath, job.FilePath);
                    var zipFilePath = PathUtils.Combine(_settingsManager.WebRootPath, "sitefiles", "dbbackup", "zip");
                    var zipFileName = $"dbback-{DateTime.Now:yyyy-MM-dd-hh-mm-ss}.zip";

                    DirectoryUtils.CreateDirectoryIfNotExists(zipFilePath);
                    var filePath = PathUtils.Combine(zipFilePath, zipFileName);

                    _pathManager.CreateZip(filePath, directionryPath);

                    var url = _pathManager.GetRootUrlByPath(filePath);

                    await _authManager.AddAdminLogAsync("下载数据库备份文件", url);
                    return new StringResult
                    {
                        Value = url
                    };
                }
                else
                {
                    return this.Error("备份文件有问题");
                }
            }
            else
            {
                return this.Error("任务有问题");
            }

        }
    }
}
