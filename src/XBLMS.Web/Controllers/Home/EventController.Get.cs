using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;

namespace XBLMS.Web.Controllers.Home
{
    public partial class EventController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var listEvent = new List<GetResultEvent>();

            var (total, list) = await _examPaperUserRepository.GetListAsync(user.Id, false, request.IsApp, "", string.Empty, 1, int.MaxValue);

            if (total > 0)
            {
                foreach (var item in list)
                {
                    var paper = await _examPaperRepository.GetAsync(item.ExamPaperId);
                    await _examManager.GetPaperInfo(paper, user);

                    listEvent.Add(new GetResultEvent
                    {
                        Id = paper.Id,
                        Title = paper.Title,
                        Start = DateUtils.GetDateAndTimeString(item.ExamBeginDateTime),
                        End = DateUtils.GetDateAndTimeString(item.ExamEndDateTime),
                        ClassNames = item.ExamEndDateTime.Value < DateTime.Now ? ["border-0"] : ["bg-warning border-0"]
                    });
                }
            }
            return new GetResult
            {
                List = listEvent,
            };
        }
    }
}
