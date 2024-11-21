using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmTreeController
    {
        [HttpPost, Route(RouteUpdate)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] ItemRequest<ExamTmTree> request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var item = await _examTmTreeRepository.GetAsync(request.Item.Id);
            item.Name = request.Item.Name;
            await _examTmTreeRepository.UpdateAsync(item);
            await _authManager.AddAdminLogAsync("修改题目分类", $"{item.Name}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
