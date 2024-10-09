using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Core.Utils;
using XBLMS.Utils;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTxController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();

            var tx = await _examTxRepository.GetAsync(request.Id);
            if (tx == null) return this.NotFound();
            var txTmCount =0;
            if (txTmCount > 0) return this.Error($"有【{txTmCount}】题目用到了该题型，暂时不允许删除");
            await _examTxRepository.DeleteAsync(request.Id);
            await _authManager.AddAdminLogAsync("删除题型", $"题型名称：{tx.Name}");
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
