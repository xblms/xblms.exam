using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Logs
{
    public partial class LogsErrorController
    {
        [HttpPost, Route(RouteExport)]
        public async Task<ActionResult<StringResult>> Export([FromBody] SearchRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Export))
            {
                return this.NoAuth();
            }
            var fileName = $"系统错误日志_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.xlsx";
            var filePath = _pathManager.GetDownloadFilesPath(fileName);
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);
            var head = new List<string>
            {
                "Id",
                "日期",
                "错误摘要",
                "错误消息",
                "错误详情"
            };
            var rows = new List<List<string>>();

            var results = await GetResultsAsync(request);
            foreach (var item in results.Items)
            {
                rows.Add(new List<string>
                {
                    item.Id.ToString(),
                    DateUtils.GetDateAndTimeString(item.CreatedDate),
                    item.Summary,
                    item.Message,
                    item.StackTrace
                });
            }

            ExcelUtils.Write(filePath, head, rows);
            var downloadUrl = _pathManager.GetDownloadFilesUrl(fileName);
            await _authManager.AddAdminLogAsync("导出系统错误日志");

            return new StringResult
            {
                Value = downloadUrl
            };
        }
    }
}
