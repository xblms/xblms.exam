using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Models;
using XBLMS.Utils;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using Microsoft.AspNetCore.Identity.Data;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTxEditController
    {
        [HttpPost, Route(RouteUpdate)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] ItemRequest<ExamTx> request)
        {
            var tx = request.Item;
            var txInfo = await _examTxRepository.GetAsync(tx.Id);

            if (txInfo.Name != tx.Name && await _examTxRepository.IsExistsAsync(tx.Name))
            {
                return this.Error("保存失败，已存在相同名称的题型！");
            }

            await _examTxRepository.UpdateAsync(tx);

            await _authManager.AddAdminLogAsync("修改题型", $"题型名称:{tx.Name}");

            return new BoolResult
            {
                Value =true
            };
        }
    }
}
