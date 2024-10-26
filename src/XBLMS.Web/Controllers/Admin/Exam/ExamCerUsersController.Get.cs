using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamCerUsersController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetUserResult>> GetUserList([FromQuery] GetUserRequest request)
        {
            var (total, list) = await _examCerUserRepository.GetListAsync(request.Id, request.Keywords, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var user = await _organManager.GetUser(item.UserId);
                    var paper = await _examPaperRepository.GetAsync(item.ExamPaperId);

                    item.Set("User", user);
                    item.Set("Paper", paper);
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
