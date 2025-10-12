using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserDocLayerViewController
    {
        [HttpGet, Route(RouteQ)]
        public async Task<ActionResult<GetExamQResult>> GetExamQ([FromQuery] GetRequest request)
        {
            var (total, list) = await _databaseManager.ExamQuestionnaireUserRepository.Analysis_GetListAsync(request.Id, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);

            if (total > 0)
            {
                foreach (var item in list)
                {
                    var qInfo = await _databaseManager.ExamQuestionnaireRepository.GetAsync(item.ExamPaperId);
                    if (qInfo == null) continue;

                    var name = qInfo.Title;
                    var prefix = "";
                    if (item.PlanId > 0)
                    {
                        var plan = await _databaseManager.StudyPlanRepository.GetAsync(item.PlanId);
                        if (plan == null) continue;

                        prefix = $"[{plan.PlanName}]";

                        if (item.CourseId > 0)
                        {
                            var courseinfo = await _databaseManager.StudyCourseRepository.GetAsync(item.CourseId);
                            if (courseinfo == null) continue;

                            prefix = prefix + $"[{courseinfo.Name}]";
                        }
                    }

                    item.Set("QName", prefix + name);
                }
            }

            return new GetExamQResult
            {
                Total = total,
                List = list
            };
        }

        public class GetExamQResult
        {
            public int Total { get; set; }
            public List<ExamQuestionnaireUser> List { get; set; }
        }
    }
}
