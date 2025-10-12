using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmGroupController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var group = await _examTmGroupRepository.GetAsync(request.Id);

            await _examTmGroupRepository.DeleteAsync(group.Id);
            await _examTmRepository.UpdateTmGroupIdsAsync(group.Id);

            await _authManager.AddAdminLogAsync("删除题目组", $"{group.GroupName}");
            await _authManager.AddStatLogAsync(StatType.ExamTmGroupDelete, "删除题目组", group.Id,group.GroupName, group);
            await _authManager.AddStatCount(StatType.ExamTmGroupDelete);
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
