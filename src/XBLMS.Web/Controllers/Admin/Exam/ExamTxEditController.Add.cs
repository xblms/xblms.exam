using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Core.Utils;
using XBLMS.Utils;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTxEditController
    {
        [HttpPost, Route(RouteAdd)]
        public async Task<ActionResult<BoolResult>> Add([FromBody] ItemRequest<ExamTx> request)
        {
            var admin = await _authManager.GetAdminAsync();
            var tx = request.Item;
            if (await _examTxRepository.IsExistsAsync(tx.Name))
            {
                return this.Error("保存失败，已存在相同名称的题型！");
            }
            tx.CompanyId = admin.CompanyId;
            tx.DepartmentId = admin.DepartmentId;
            tx.CreatorId = admin.Id;

            await _examTxRepository.InsertAsync(tx);
            await _authManager.AddAdminLogAsync("添加题型", $"题型名称:{tx.Name}");
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
