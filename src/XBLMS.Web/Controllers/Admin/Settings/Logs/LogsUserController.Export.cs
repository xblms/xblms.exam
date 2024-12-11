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
    public partial class LogsUserController
    {
        [HttpPost, Route(RouteExport)]
        public async Task<ActionResult<StringResult>> Export([FromBody] SearchRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Export))
            {
                return this.NoAuth();
            }

            var fileName = $"用户日志_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.xlsx";
            var filePath = _pathManager.GetDownloadFilesPath(fileName);
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);
            var head = new List<string>
            {
                "用户",
                "IP地址",
                "日期",
                "动作",
                "描述"
            };
            var rows = new List<List<string>>();

            var results = await GetResultsAsync(request);
            foreach (var item in results.Items)
            {
                rows.Add(new List<string>
                {
                    item.Get<string>("userName"),
                    item.IpAddress,
                    DateUtils.GetDateAndTimeString(item.CreatedDate),
                    item.Action,
                    item.Summary
                });
            }

            ExcelUtils.Write(filePath, head, rows);
            var downloadUrl = _pathManager.GetDownloadFilesUrl(fileName);

            await _authManager.AddAdminLogAsync("导出用户日志");
            await _authManager.AddStatLogAsync(StatType.Export, "导出用户日志", 0, string.Empty, new StringResult { Value = downloadUrl });
            return new StringResult
            {
                Value = downloadUrl
            };
        }
    }
}
