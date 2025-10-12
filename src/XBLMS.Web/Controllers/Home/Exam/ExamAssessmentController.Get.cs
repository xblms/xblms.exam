using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamAssessmentController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var resultList = new List<ExamAssessment>();
            var (total, list) = await _examAssessmentUserRepository.GetListAsync(user.Id, request.KeyWords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var assInfo = await _examAssessmentRepository.GetAsync(item.ExamAssId);
                    _examManager.GetExamAssessmentInfo(assInfo, item, user);
                    resultList.Add(assInfo);
                }
            }
            return new GetResult
            {
                Total = total,
                List = resultList
            };
        }

        [HttpGet, Route(RouteItem)]
        public async Task<ActionResult<GetItemResult>> GetItem([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var assInfo = await _examAssessmentRepository.GetAsync(request.Id);
            var assUser = await _examAssessmentUserRepository.GetAsync(assInfo.Id, user.Id);

            _examManager.GetExamAssessmentInfo(assInfo, assUser, user);

            var pointNotice = await _authManager.PointNotice(PointType.PointExamAss, user.Id);

            return new GetItemResult
            {
                PointNotice = pointNotice,
                Item = assInfo
            };
        }
    }
}
