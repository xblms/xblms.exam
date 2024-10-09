using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Models;
using XBLMS.Utils;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using Microsoft.AspNetCore.Identity.Data;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperTreeController
    {
        [HttpPost, Route(RouteUpdate)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] ItemRequest<ExamPaperTree> request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var item = await _examPaperTreeRepository.GetAsync(request.Item.Id);
            item.Name = request.Item.Name;
            await _examPaperTreeRepository.UpdateAsync(item);
            await _authManager.AddAdminLogAsync("修改试卷分类", $"分类名称:{item.Name}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
