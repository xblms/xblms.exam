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

            if (tx.ExamTxBase == Enums.ExamTxBase.Zuheti)
            {
                var allRight = true;
                foreach (var small in request.SmallList)
                {
                    var smallTm = await _examTmSmallRepository.GetAsync(small.Id);
                    var smallTx = await _examTxRepository.GetAsync(smallTm.TxId);

                    var smallIsRight = IsRight(smallTx, smallTm, small.Answer);
                    if (!smallIsRight)
                    {
                        allRight = false;
                    }
                    var smallAnswerInfo = new ExamPracticeAnswerSmall
                    {
                        UserId = user.Id,
                        PracticeId = request.PracticeId,
                        TmId = small.Id,
                        IsRight = smallIsRight,
                        Answer = small.Answer,
                        AnswerId = request.Id
                    };
                    smallAnswerInfo.Set("optionsValues", small.AnswerValues);
                    await _examPracticeAnswerSmallRepository.InsertAsync(smallAnswerInfo);
                }
                if (allRight)
                {
                    result.IsRight = true;
                }
            }
            else
            {
                result.IsRight = IsRight(tx, tm, request.Answer);
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

            if (result.IsRight)
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


            await _examPracticeRepository.IncrementAnswerCountAsync(request.PracticeId);
            if (result.IsRight)
            {
                await _examPracticeRepository.IncrementRightCountAsync(request.PracticeId);
            }

            return result;
        }
        private bool IsRight(ExamTx tx, ExamTm tm, string myAnswer)
        {
            var isRight = false;
            if (tx.ExamTxBase == Enums.ExamTxBase.Tiankongti || tx.ExamTxBase == Enums.ExamTxBase.Jiandati)
            {
                if (StringUtils.EqualsIgnoreCase(tm.Answer, myAnswer))
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
                    if (!StringUtils.ContainsIgnoreCase(myAnswer, answer))
                    {
                        allTrue = false;
                    }
                }
                if (!allTrue)
                {
                    answerList = ListUtils.GetStringList(tm.Answer, ";");
                    foreach (var answer in answerList)
                    {
                        if (!StringUtils.ContainsIgnoreCase(myAnswer, answer))
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
                        if (!StringUtils.ContainsIgnoreCase(myAnswer, answer))
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
                        if (!StringUtils.ContainsIgnoreCase(myAnswer, answer))
                        {
                            allTrue = false;
                        }
                    }
                }
                isRight = allTrue;
            }
            return isRight;
        }
    }

}



