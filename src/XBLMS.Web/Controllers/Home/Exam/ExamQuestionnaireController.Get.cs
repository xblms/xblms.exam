using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamQuestionnaireController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var resultList = new List<ExamQuestionnaire>();
            var (total, list) = await _examQuestionnaireUserRepository.GetListAsync(user.Id, request.KeyWords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var paper = await _examQuestionnaireRepository.GetAsync(item.ExamPaperId);
                    await _examManager.GetQuestionnaireInfo(paper, user);
                    resultList.Add(paper);
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

            var paper = await _examQuestionnaireRepository.GetAsync(request.Id);
            await _examManager.GetQuestionnaireInfo(paper, user);

            var pointNotice = await _authManager.PointNotice(PointType.PointExamQ, user.Id);

            return new GetItemResult
            {
                PointNotice = pointNotice,
                Item = paper
            };
        }
    }
}
