using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTxEditController
    {
        [HttpPost, Route(RouteUpdate)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] ItemRequest<ExamTx> request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var tx = request.Item;
            var txInfo = await _examTxRepository.GetAsync(tx.Id);

            if (txInfo.Name != tx.Name && await _examTxRepository.IsExistsAsync(tx.Name))
            {
                return this.Error("保存失败，已存在相同名称的题型！");
            }

            await _examTxRepository.UpdateAsync(tx);

            await _authManager.AddAdminLogAsync("修改题型", $"{tx.Name}");
            await _authManager.AddStatLogAsync(StatType.ExamTxUpdate, "修改题型", txInfo.Id, txInfo.Name, txInfo);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
