using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Study
{
    public partial class StudyCourseInfoController
    {
        [HttpGet, Route(RouteEvaluation)]
        public async Task<ActionResult<GetEvaluationResult>> GetEvaluation([FromQuery] GetEvaluationRequest request)
        {
            var starList = new List<int>() { 0, 0, 0, 0, 0, 0 };
            var starAvg = "";
            int starUser = 0;
            if (request.PageIndex <= 1)
            {
                var fiveStar = await _studyCourseUserRepository.GetAvgEvaluationAsync(request.CourseId, 5);
                var fourStar = await _studyCourseUserRepository.GetAvgEvaluationAsync(request.CourseId, 4);
                var threeStar = await _studyCourseUserRepository.GetAvgEvaluationAsync(request.CourseId, 3);
                var twoStar = await _studyCourseUserRepository.GetAvgEvaluationAsync(request.CourseId, 2);
                var oneStar = await _studyCourseUserRepository.GetAvgEvaluationAsync(request.CourseId, 1);
                starList = new List<int> { fiveStar, fourStar, threeStar, twoStar, oneStar };
                var course = await _studyCourseRepository.GetAsync(request.CourseId);

                starUser = course.TotaEvaluationlUser;
                starAvg = TranslateUtils.ToAvg(Convert.ToDouble(course.TotalAvgEvaluation), starUser);
            }


            var (total, list) = await _studyCourseEvaluationUserRepository.GetListAsync(request.CourseId, request.PageIndex, request.PageSize);
            var resultList = new List<GetEvaluationResultInfo>();

            if (total > 0)
            {
                foreach (var item in list)
                {
                    var user = await _organManager.GetUser(item.UserId);
                    if (user == null) continue;
                    var eInfo = await _studyCourseEvaluationRepository.GetAsync(item.EvaluationId);
                    if(eInfo==null) continue;
                    var courseUser = await _studyCourseUserRepository.GetAsync(user.Id, request.PlanId, request.CourseId);
                    if(courseUser == null) continue;
                    var eitems = await _studyCourseEvaluationItemUserRepository.GetTextAsync(request.CourseId, item.UserId);

                    resultList.Add(new GetEvaluationResultInfo
                    {
                        DisplayName = user?.DisplayName,
                        AvatarUrl = user?.AvatarUrl,
                        Star = courseUser.AvgEvaluation,
                        StarDateTime = item.CreatedDate.Value,
                        TextContent = eitems,
                        MaxStar = eInfo.MaxStar
                    });
                }
            }

            return new GetEvaluationResult
            {
                StarAvg = starAvg,
                StarUser = starUser,
                StarList = starList,
                Total = total,
                List = resultList
            };
        }
    }
}
