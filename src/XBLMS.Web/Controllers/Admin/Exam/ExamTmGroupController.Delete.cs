using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmGroupController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            var group = await _examTmGroupRepository.GetAsync(request.Id);

            await _examTmGroupRepository.DeleteAsync(group.Id);

            await _authManager.AddAdminLogAsync("删除题目组", $"题目组名称:{group.GroupName}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
