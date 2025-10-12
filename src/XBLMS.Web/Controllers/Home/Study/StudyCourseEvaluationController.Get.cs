using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Home.Study
{
    public partial class StudyCourseEvaluationController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }


            var title = "";
            if (request.PlanId > 0)
            {
                var courseInfo = await _studyPlanCourseRepository.GetAsync(request.PlanId, request.CourseId);
                title = courseInfo.CourseName;
            }
            else
            {
                var course = await _studyCourseRepository.GetAsync(request.CourseId);
                title = course.Name;
            }

            var evaluation = await _studyCourseEvaluationRepository.GetAsync(request.EId);

            var list = await _studyCourseEvaluationItemRepository.GetListAsync(request.EId);

            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.Set("StarValue", 0);
                    item.Set("StrValue", "");
                }
            }
            return new GetResult()
            {
                MaxStar = evaluation.MaxStar,
                Title = title,
                List = list
            };
        }

    }
}
