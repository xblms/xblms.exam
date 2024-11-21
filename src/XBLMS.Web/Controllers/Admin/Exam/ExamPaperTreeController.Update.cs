using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

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
            await _authManager.AddAdminLogAsync("修改试卷分类", $"{item.Name}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
