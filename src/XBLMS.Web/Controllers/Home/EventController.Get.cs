using Microsoft.AspNetCore.Mvc;
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

            var (total, list) = await _examPaperUserRepository.GetEventListAsync(user.Id, request.IsApp);

            //考试
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var paper = await _examPaperRepository.GetAsync(item.ExamPaperId);
                    await _examManager.GetPaperInfo(paper, user);
                    if (!paper.IsCourseUse)
                    {
                        listEvent.Add(new GetResultEvent
                        {
                            GroupId = "exam",
                            Id = paper.Id,
                            Title = $"【考试】{paper.Title}",
                            Start = DateUtils.GetDateAndTimeString(item.ExamBeginDateTime),
                            End = DateUtils.GetDateAndTimeString(item.ExamEndDateTime),
                            ClassNames = ["border-0 rounded-0"]
                        });
                    }
                }
            }
            //面授课
            var (offTotal, offList) = await _studyCourseRepository.User_GetPublicEventListAsync(user.CompanyId);
            if (offTotal > 0)
            {
                foreach (var item in offList)
                {
                    listEvent.Add(new GetResultEvent
                    {
                        GroupId = "offline",
                        Id = item.Id,
                        Title = $"【面授课】{item.Name}",
                        Start = DateUtils.GetDateAndTimeString(item.OfflineBeginDateTime),
                        End = DateUtils.GetDateAndTimeString(item.OfflineEndDateTime),
                        ClassNames = ["border-0 rounded-0"]
                    });
                }
            }

            //计划内面授课
            var (planOffTotal, planOffList) = await _studyCourseUserRepository.GetOfflineListByEventAsync(user.Id);
            if (planOffTotal > 0)
            {
                foreach (var item in planOffList)
                {
                    var course = await _studyCourseRepository.GetAsync(item.CourseId);
                    listEvent.Add(new GetResultEvent
                    {
                        allow = item.PlanId,
                        GroupId = "offline",
                        Id = course.Id,
                        Title = $"【计划内面授课】{course.Name}",
                        Start = DateUtils.GetDateAndTimeString(course.OfflineBeginDateTime),
                        End = DateUtils.GetDateAndTimeString(course.OfflineEndDateTime),
                        ClassNames = ["border-0 rounded-0"]
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
