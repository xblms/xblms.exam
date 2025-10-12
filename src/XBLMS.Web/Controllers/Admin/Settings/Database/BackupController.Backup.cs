using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Database
{
    public partial class BackupController
    {
        [HttpPost, Route(RouteBackupDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var job = await _dbBackupRepository.GetAsync(request.Id);
            if (job != null)
            {
                if (!string.IsNullOrWhiteSpace(job.FilePath))
                {
                    var directionryPath = PathUtils.Combine(_settingsManager.WebRootPath, job.FilePath);
                    DirectoryUtils.DeleteDirectoryIfExists(directionryPath);
                }
            }
            await _authManager.AddAdminLogAsync("删除数据库备份", $"备份时间：{job.EndTime}");
            await _dbBackupRepository.DeleteAsync(request.Id);
            return new BoolResult
            {
                Value = true
            };
        }
        [HttpGet, Route(RouteBackupExcution)]
        public async Task<ActionResult<BoolResult>> Excution()
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
            {
                return this.NoAuth();
            }

            var jobinfo = new DbBackup
            {
                BeginTime = DateTime.Now,
                Status = 0
            };
            await _dbBackupRepository.InsertAsync(jobinfo);

            return new BoolResult
            {
                Value = true
            };
        }

        [HttpPost, Route(RouteBackup)]
        public async Task<ActionResult<GetResult>> GetLog([FromBody] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var config = await _configRepository.GetAsync();
            var existsBackup = await _dbBackupRepository.ExistsBackupingAsync();
            var (total, list) = await _dbBackupRepository.GetListAsync(request.PageIndex, request.PageSize);
            return new GetResult
            {
                DbBackupAuto = config.DbBackupAuto,
                ExistsBackup = existsBackup,
                Total = total,
                List = list
            };
        }

        [HttpPost, Route(RouteBackupConfig)]
        public async Task<ActionResult<BoolResult>> SetConfig([FromBody] GetSetConfigRequest request)
        {
            var config = await _configRepository.GetAsync();
            config.DbBackupAuto = request.DbBackupAuto;

            await _configRepository.UpdateAsync(config);
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
