using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils.Office;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmController
    {
        [HttpGet, Route(RouteExportExcel)]
        public async Task<ActionResult<StringResult>> Export([FromQuery] GetSearchRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Export))
            {
                return this.NoAuth();
            }
            var adminAuth = await _authManager.GetAdminAuth();
            var treeIds = new List<int>();
            if (request.TreeId > 0)
            {
                if (request.TreeIsChildren)
                {
                    treeIds = await _examTmTreeRepository.GetIdsAsync(request.TreeId);
                }
                else
                {
                    treeIds.Add(request.TreeId);
                }
            }
            var group = await _examTmGroupRepository.GetAsync(request.TmGroupId);

            var (total, list) = await _examTmRepository.GetListAsync(adminAuth, group, treeIds, request.TxId, request.Nandu, request.Keyword, request.Order, request.OrderType, request.IsStop, 1, int.MaxValue);

            var fileName = "题目列表.xlsx";
            var filePath = _pathManager.GetDownloadFilesPath(fileName);

            var excelObject = new ExcelObject(_databaseManager, _pathManager, _organManager, _examManager);
            await excelObject.CreateExcelFileForTmAsync(list, filePath);

            var downloadUrl = _pathManager.GetRootUrlByPath(filePath);

            await _authManager.AddAdminLogAsync("导出题目");
            await _authManager.AddStatLogAsync(StatType.Export, "导出题目", 0, string.Empty, new StringResult { Value = downloadUrl });

            return new StringResult
            {
                Value = downloadUrl
            };
        }



    }
}
