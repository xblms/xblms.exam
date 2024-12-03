using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireUsersController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetUserResult>> GetUserList([FromQuery] GetUserRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }

            var (total, list) = await _examQuestionnaireUserRepository.GetListAsync(request.Id, request.IsSubmit, request.Keywords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var user = await _organManager.GetUser(item.UserId);

                    item.Set("User", user);
                }
            }
            return new GetUserResult
            {
                Total = total,
                List = list
            };
        }
    }
}
