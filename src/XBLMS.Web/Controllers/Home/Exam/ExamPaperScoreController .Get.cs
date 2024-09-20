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

            var paperIds = new List<int>();
            if (!string.IsNullOrWhiteSpace(request.KeyWords))
            {
                var myPaperIds = await _examPaperStartRepository.GetPaperIdsAsync(user.Id);
                if (myPaperIds == null || myPaperIds.Count == 0)
                {
                    return new GetResult
                    {
                        Total = 0,
                        List = null
                    };
                }
                paperIds = await _examPaperRepository.GetIdsAsync(myPaperIds, request.KeyWords);

                if (paperIds == null || paperIds.Count == 0)
                {
                    return new GetResult
                    {
                        Total = 0,
                        List = null
                    };
                }
            }

            var (total, list) = await _examPaperStartRepository.GetListAsync(paperIds, user.Id, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var paper = await _examPaperRepository.GetAsync(item.ExamPaperId);
                    await _examManager.GetPaperInfo(paper, user);
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
