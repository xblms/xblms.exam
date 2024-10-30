using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperScoreController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var (total, list) = await _examPaperStartRepository.GetListAsync(user.Id, request.DateFrom, request.DateTo, request.KeyWords, request.PageIndex, request.PageSize);

            if (total > 0)
            {
                foreach (var item in list)
                {
                    var paper = await _examPaperRepository.GetAsync(item.ExamPaperId);
                    await _examManager.GetPaperInfo(paper, user, item);
                    item.Set("Paper", paper);

                    if (!paper.SecrecyScore)
                    {
                        item.Score = 0;
                    }
                    item.Set("UseTime", DateUtils.SecondToHms(item.ExamTimeSeconds));
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
