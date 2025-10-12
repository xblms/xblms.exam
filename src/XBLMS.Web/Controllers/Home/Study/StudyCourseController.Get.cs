using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Home.Study
{
    public partial class StudyCourseController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var resultList = new List<StudyCourse>();
            var resultTotal = 0;
            var resultMarkTotal = 0;
            var resultMarkList = new List<string>();

            if (request.Collection || !string.IsNullOrEmpty(request.State))
            {
                var (markTotal, markList) = await _studyCourseUserRepository.GetMarkListAsync(user.Id);
                resultMarkTotal = markTotal;
                resultMarkList = markList;

                var (total, list) = await _studyCourseUserRepository.GetListAsync(user.Id, request.Collection, request.KeyWords, request.Mark, request.Orderby, request.State, request.PageIndex, request.PageSize);
                resultTotal = total;
                if (total > 0)
                {
                    foreach (var item in list)
                    {
                        var course = await _studyCourseRepository.GetAsync(item.CourseId);
                        await _studyManager.User_GetCourseInfoByCourseList(item.PlanId, course, item);
                        resultList.Add(course);
                    }
                }
            }
            else
            {
                var (markTotal, markList) = await _studyCourseRepository.User_GetPublicMarkListAsync(user.CompanyId);
                resultMarkTotal = markTotal;
                resultMarkList = markList;

                var (total, list) = await _studyCourseRepository.User_GetPublicListAsync(user.CompanyId, request.KeyWords, request.Mark, request.Orderby, request.PageIndex, request.PageSize);
                resultTotal = total;
                if (total > 0)
                {
                    foreach (var item in list)
                    {
                        var courseUser = await _studyCourseUserRepository.GetAsync(user.Id, 0, item.Id);
                        await _studyManager.User_GetCourseInfoByCourseList(0, item, courseUser);
                        resultList.Add(item);
                    }
                }
            }


            return new GetResult
            {
                MarkTotal = resultMarkTotal,
                MarkList = resultMarkList,
                Total = resultTotal,
                List = resultList
            };
        }

        [HttpGet, Route(RouteItem)]
        public async Task<ActionResult<ItemResult<StudyCourse>>> GetItem([FromQuery] GetItemRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var course = await _studyCourseRepository.GetAsync(request.Id);
            var courseUser = await _studyCourseUserRepository.GetAsync(user.Id, request.PlanId, request.Id);

            await _studyManager.User_GetCourseInfoByCourseList(request.PlanId, course, courseUser);

            return new ItemResult<StudyCourse>
            {
                Item = course
            };
        }
    }
}
