using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPkUsersController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetUserResult>> GetUserList([FromQuery] GetUserRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }

            var pk = await _examPkRepository.GetAsync(request.Id);

            var (total, list) = await _examPkUserRepository.GetListAsync(request.Id, request.KeyWords);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var user = await _organManager.GetUser(item.UserId);

                    item.Set("User", user);
                    item.Set("UseTime", DateUtils.SecondToHms(item.DurationTotal));
                }
            }
            return new GetUserResult
            {
                Title = pk.Name,
                Total = total,
                List = list
            };
        }
    }
}
