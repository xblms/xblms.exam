using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseTreeController
    {
        [HttpPost, Route(RouteUpdate)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] ItemRequest<ExamPaperTree> request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var item = await _studyCourseTreeRepository.GetAsync(request.Item.Id);
            item.Name = request.Item.Name;
            await _studyCourseTreeRepository.UpdateAsync(item);
            await _authManager.AddAdminLogAsync("修改课程分类", $"{item.Name}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
