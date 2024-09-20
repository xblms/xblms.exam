using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeLogController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();

            var (total, list) = await _examPracticeRepository.GetListAsync(user.Id, request.DateFrom,request.DateTo, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    item.TmIds = null;
                    item.Zsds = null;
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
