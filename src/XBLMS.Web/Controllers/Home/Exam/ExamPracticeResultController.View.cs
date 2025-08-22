using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeResultController
    {
        [HttpGet, Route(RouteView)]
        public async Task<ActionResult<GetViewResult>> GetView([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var tmList = new List<ExamTm>();
            var practice = await _examPracticeRepository.GetAsync(request.Id);
            if (practice.AnswerCount > 0)
            {
                var answerList = await _examPracticeAnswerRepository.GetListAsync(practice.Id);
                foreach (var answer in answerList)
                {
                    var tm = await _examManager.GetTmInfo(answer.TmId);
                    await _examManager.GetTmInfoByPracticeView(tm, practice.Id);
                    tm.Set("AnswerInfo", answer);
                    tmList.Add(tm);
                }
            }
            return new GetViewResult
            {
                Item = practice,
                TmList = tmList
            };
        }
    }
}
