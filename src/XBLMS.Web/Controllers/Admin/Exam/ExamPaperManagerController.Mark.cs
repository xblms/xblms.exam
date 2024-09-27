using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperManagerController
    {
        [HttpGet, Route(RouteMark)]
        public async Task<ActionResult<GetScoreResult>> GetMarkList([FromQuery] GetSocreRequest request)
        {
            var (total, list) = await _examPaperStartRepository.GetListByAdminAsync(request.Id, request.DateFrom, request.DateTo, request.Keywords, request.PageIndex, request.PageSize, false);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var user = await _organManager.GetUser(item.UserId);

                    user.Set("UseTime", DateUtils.SecondToHms(item.ExamTimeSeconds));

                    item.Set("User", user);
                }
            }
            return new GetScoreResult
            {
                Total = total,
                List = list
            };
        }
    }
}
