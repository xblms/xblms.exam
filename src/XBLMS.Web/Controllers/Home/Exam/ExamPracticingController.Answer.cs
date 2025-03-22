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
            var tx = await _examTxRepository.GetAsync(tm.TxId);

            var result = new GetSubmitAnswerResult
            {
                IsRight = false,
                Answer = tm.Answer,
                Jiexi = tm.Jiexi
            };

            var isRight = false;
            if (tx.ExamTxBase == Enums.ExamTxBase.Tiankongti || tx.ExamTxBase == Enums.ExamTxBase.Jiandati)
            {
                if (StringUtils.EqualsIgnoreCase(tm.Answer, request.Answer))
                {
                    isRight = true;
                }
            }
            else
            {
                var answerList = ListUtils.GetStringList(tm.Answer);
                var allTrue = true;
                foreach (var answer in answerList)
                {
                    if (!StringUtils.ContainsIgnoreCase(request.Answer, answer))
                    {
                        allTrue = false;
                    }
                }
                if (!allTrue)
                {
                    answerList = ListUtils.GetStringList(tm.Answer, ";");
                    foreach (var answer in answerList)
                    {
                        if (!StringUtils.ContainsIgnoreCase(request.Answer, answer))
                        {
                            allTrue = false;
                        }
                    }
                }
                if (!allTrue)
                {
                    answerList = ListUtils.GetStringList(tm.Answer, "，");
                    foreach (var answer in answerList)
                    {
                        if (!StringUtils.ContainsIgnoreCase(request.Answer, answer))
                        {
                            allTrue = false;
                        }
                    }
                }
                if (!allTrue)
                {
                    answerList = ListUtils.GetStringList(tm.Answer, "；");
                    foreach (var answer in answerList)
                    {
                        if (!StringUtils.ContainsIgnoreCase(request.Answer, answer))
                        {
                            allTrue = false;
                        }
                    }
                }
                isRight = allTrue;
            }

            if (isRight)
            {
                result.IsRight = true;
                result.Answer = string.Empty;
                result.Jiexi = string.Empty;
            }
            else
            {
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
            }


            var answerInfo = new ExamPracticeAnswer
            {
                UserId = user.Id,
                PracticeId = request.PracticeId,
                TmId = request.Id,
                IsRight = result.IsRight,
                Answer = request.Answer,
            };
            answerInfo.Set("optionsValues", request.AnswerValues);
            await _examPracticeAnswerRepository.InsertAsync(answerInfo);


            await _examPracticeRepository.IncrementAnswerCountAsync(request.PracticeId);
            if (result.IsRight)
            {
                await _examPracticeRepository.IncrementRightCountAsync(request.PracticeId);
            }

            return result;
        }

    }
}



