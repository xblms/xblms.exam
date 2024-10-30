using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticingController
    {
        [HttpPost, Route(RouteAnswer)]
        public async Task<ActionResult<GetSubmitAnswerResult>> Answer([FromBody] GetSubmitAnswerRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var tm = await _examTmRepository.GetAsync(request.Id);

            var result = new GetSubmitAnswerResult
            {
                IsRight = false,
                Answer = tm.Answer,
                Jiexi = tm.Jiexi
            };

            if (StringUtils.Equals(tm.Answer, request.Answer))
            {
                result.IsRight = true;
                result.Answer = "";
                result.Jiexi = "";
            }

            await _examPracticeAnswerRepository.InsertAsync(new ExamPracticeAnswer
            {
                UserId = user.Id,
                PracticeId = request.PracticeId,
                TmId = request.Id,
                IsRight = result.IsRight
            });

            var wrong = await _examPracticeWrongRepository.GetAsync(user.Id);
            if (wrong != null)
            {
                if (!wrong.TmIds.Contains(request.Id))
                {
                    wrong.TmIds.Add(request.Id);
                    await _examPracticeWrongRepository.UpdateAsync(wrong);
                }
            }
            else
            {
                await _examPracticeWrongRepository.InsertAsync(new ExamPracticeWrong
                {
                    UserId = user.Id,
                    TmIds = new List<int> { request.Id }
                });
            }

            await _examPracticeRepository.IncrementAnswerCountAsync(request.PracticeId);
            if (result.IsRight)
            {
                await _examPracticeRepository.IncrementRightCountAsync(request.PracticeId);
            }

            return result;
        }

    }
}



