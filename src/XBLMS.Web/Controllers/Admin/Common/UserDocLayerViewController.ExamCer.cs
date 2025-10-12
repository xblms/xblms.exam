using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserDocLayerViewController
    {
        [HttpGet, Route(RouteExamCer)]
        public async Task<ActionResult<GetExamCerResult>> GetExamCer([FromQuery] GetRequest request)
        {
            var (total, list) = await _databaseManager.ExamCerUserRepository.Analysis_GetListAsync(request.Id, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);

            if (total > 0)
            {
                foreach (var item in list)
                {
                    var cer = await _databaseManager.ExamCerRepository.GetAsync(item.CerId);
                    if (cer == null) continue;
                    item.Set("CerName", cer.Name);

                    var sourceList = new List<string>();
                    if (item.PlanId > 0)
                    {
                        var plan=await _databaseManager.StudyPlanRepository.GetAsync(item.PlanId);
                        if (plan == null) continue;
                        sourceList.Add(plan.PlanName);
                    }
                    if (item.CourseId > 0)
                    {
                        var course=await _databaseManager.StudyCourseRepository.GetAsync(item.CourseId);
                        if (course == null) continue;
                        sourceList.Add(course.Name);
                    }
                    var paper = await _databaseManager.ExamPaperRepository.GetAsync(item.ExamPaperId);
                    if (paper == null) continue;
                    sourceList.Add(paper.Title);

                    item.Set("Source", ListUtils.ToString(sourceList, "/"));
                }
            }

            return new GetExamCerResult
            {
                Total = total,
                List = list
            };
        }

        public class GetExamCerResult
        {
            public int Total { get; set; }
            public List<ExamCerUser> List { get; set; }
        }
    }
}
