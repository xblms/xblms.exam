using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
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
            var jobTime = await Backup();
            await _authManager.AddAdminLogAsync("备份数据库", $"备份时间：{jobTime}");
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
            var (total, list) = await _dbBackupRepository.GetListAsync(request.PageIndex, request.PageSize);
            return new GetResult
            {
                Total = total,
                List = list

            };
        }
    }
}
