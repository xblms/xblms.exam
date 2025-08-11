using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamQuestionnairingController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteSubmitPaper)]
        public async Task<ActionResult<BoolResult>> SubmitPaper([FromBody] GetSubmitRequest request)
        {
            var paper = await _examQuestionnaireRepository.GetAsync(request.Id);
            if (paper == null)
            {
                return this.NotFound();
            }

            var user = new User();
            user.Id = 0;

            if (!paper.Published)
            {
                user = await _authManager.GetUserAsync();

                var paperUser = await _examQuestionnaireUserRepository.GetAsync(request.Id, user.Id);
                paperUser.SubmitType = SubmitType.Submit;
                await _examQuestionnaireUserRepository.UpdateAsync(paperUser);
            }
          

            if (request.TmList != null && request.TmList.Count > 0)
            {
                foreach (var item in request.TmList)
                {
                    await _examQuestionnaireAnswerRepository.InsertAsync(new ExamQuestionnaireAnswer
                    {
                        TmId = item.Id,
                        Answer = item.Get("Answer").ToString(),
                        ExamPaperId = request.Id,
                        UserId = user.Id
                    });
                }
            }

            await _examQuestionnaireRepository.IncrementAsync(request.Id);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
