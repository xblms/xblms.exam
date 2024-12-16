using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamAssessmentingController
    {
        [HttpPost, Route(RouteSubmitPaper)]
        public async Task<ActionResult<BoolResult>> SubmitPaper([FromBody] GetSubmitRequest request)
        {
            var assInfo = await _examAssessmentRepository.GetAsync(request.Id);
            if (assInfo == null)
            {
                return this.NotFound();
            }

            var user = await _authManager.GetUserAsync();

            var assUser = await _examAssessmentUserRepository.GetAsync(request.Id, user.Id);
            assUser.SubmitType = SubmitType.Submit;
            await _examAssessmentUserRepository.UpdateAsync(assUser);


            if (request.TmList != null && request.TmList.Count > 0)
            {
                var config = await _examAssessmentConfigRepository.GetAsync(assInfo.ConfigId);
                var configList = await _examAssessmentConfigSetRepository.GetListAsync(config.Id);

                var totalScore = 0;
                foreach (var item in request.TmList)
                {
                    var optionScoreList = TranslateUtils.JsonDeserialize<List<int>>(item.Get("OptionsScore").ToString());
                    var optionRandomList = TranslateUtils.JsonDeserialize<List<KeyValuePair<string, string>>>(item.Get("OptionsRandom").ToString());

                    var answer = item.Get("Answer").ToString();

                    try
                    {
                        for (var i = 0; i < optionRandomList.Count; i++)
                        {
                            if (answer.Contains(optionRandomList[i].Key))
                            {
                                totalScore += optionScoreList[i];
                            }
                        }

                    }
                    catch { }

                    await _examAssessmentAnswerRepository.InsertAsync(new ExamAssessmentAnswer
                    {
                        TmId = item.Id,
                        Answer = answer,
                        ExamAssId = request.Id,
                        UserId = user.Id
                    });
                }

                foreach (var item in configList)
                {
                    if (totalScore >= item.MinScore && totalScore <= item.MaxScore)
                    {
                        assUser.ConfigId = item.Id; ;
                        assUser.ConfigName = item.Result;
                        assUser.TotalScore = totalScore;
                        await _examAssessmentUserRepository.UpdateAsync(assUser);
                        break;
                    }
                }
            }

            await _examAssessmentRepository.IncrementAsync(request.Id);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
