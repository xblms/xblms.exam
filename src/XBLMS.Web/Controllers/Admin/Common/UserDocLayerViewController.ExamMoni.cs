using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserDocLayerViewController
    {
        [HttpGet, Route(RouteExamMoni)]
        public async Task<ActionResult<GetExamMoniResult>> GetExamMoni([FromQuery] GetRequest request)
        {
            var (total, list) = await _databaseManager.ExamPaperStartRepository.Analysis_GetListAsync(request.Id, true, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);

            if (total > 0)
            {
                foreach (var item in list)
                {
                    var paper = await _databaseManager.ExamPaperRepository.GetAsync(item.ExamPaperId);
                    if (paper == null) continue;
                    var name = paper.Title;
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
                    item.Set("UseTime", TranslateUtils.ToMinuteAndSecond(item.ExamTimeSeconds));
                    item.Set("ExamName", prefix + name);
                }
            }

            return new GetExamMoniResult
            {
                Total = total,
                List = list
            };
        }

        public class GetExamMoniResult
        {
            public int Total { get; set; }
            public List<ExamPaperStart> List { get; set; }
        }
    }
}
