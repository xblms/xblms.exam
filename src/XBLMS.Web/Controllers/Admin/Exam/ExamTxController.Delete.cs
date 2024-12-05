using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

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
            var txTmCount = 0;
            if (txTmCount > 0) return this.Error($"有【{txTmCount}】题目用到了该题型，暂时不允许删除");
            await _examTxRepository.DeleteAsync(request.Id);

            await _authManager.AddAdminLogAsync("删除题型", $"{tx.Name}");
            await _authManager.AddStatLogAsync(StatType.ExamTxDelete, "删除题型", tx.Id, tx.Name, tx);
            await _authManager.AddStatCount(StatType.ExamTxDelete);
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
