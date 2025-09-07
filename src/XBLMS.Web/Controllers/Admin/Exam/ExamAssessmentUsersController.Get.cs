using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamAssessmentUsersController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetUserResult>> GetUserList([FromQuery] GetUserRequest request)
        {
            var (total, list) = await _examAssessmentUserRepository.GetListAsync(request.Id, request.IsSubmit, request.Keywords, request.PageIndex, request.PageSize);
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
