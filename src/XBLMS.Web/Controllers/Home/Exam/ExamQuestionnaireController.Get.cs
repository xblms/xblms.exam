using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamQuestionnaireController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();

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
        public async Task<ActionResult<ItemResult<ExamQuestionnaire>>> GetItem([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            var paper = await _examQuestionnaireRepository.GetAsync(request.Id);
            await _examManager.GetQuestionnaireInfo(paper, user);
            return new ItemResult<ExamQuestionnaire>
            {
                Item = paper
            };
        }
    }
}
