using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamTmCorrectionController
    {
        [HttpGet, Route(RouteLog)]
        public async Task<ActionResult<GetResult>> GetLogs([FromQuery] GetLogRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var (total, list) = await _examTmCorrectionRepository.GetListAsync(user.Id, request.Keywords, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    item.Set("AuditStatusStr", item.AuditStatus.GetDisplayName());
                }
            }

            return new GetResult
            {
                Total = total,
                List = list
            };
        }
    }
}
