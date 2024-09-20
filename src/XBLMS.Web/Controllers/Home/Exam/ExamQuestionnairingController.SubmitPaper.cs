using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamQuestionnairingController
    {
        [HttpPost, Route(RouteSubmitPaper)]
        public async Task<BoolResult> SubmitPaper([FromBody] GetSubmitRequest request)
        {
            var user = await _authManager.GetUserAsync();

            if (request.TmList != null && request.TmList.Count > 0)
            {
                foreach (var item in request.TmList)
                {
                    await _examQuestionnaireAnswerRepository.InsertAsync(new ExamQuestionnaireAnswer
                    {
                        TmId = item.Id,
                        Answer = item.Get("Answer").ToString(),
                        ExamPaperId = request.PapaerId,
                        UserId = user.Id
                    });
                }
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
