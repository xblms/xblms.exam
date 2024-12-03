using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireAnalysisController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }

            var paper = await _questionnaireRepository.GetAsync(request.Id);
            var tmList = await _questionnaireTmRepository.GetListAsync(paper.Id);

            var tmTotal = 0;
            if (tmList != null && tmList.Count > 0)
            {
                foreach (var tm in tmList)
                {
                    tmTotal++;
                    var tmAnswerToTal = 0;
                    var optionAnswer = new List<string>();
                    if (tm.Tx == ExamQuestionnaireTxType.Jiandati)
                    {
                        var answerList = await _questionnaireAnswerRepository.GetListAnswer(paper.Id, tm.Id);
                        if (answerList != null && answerList.Count > 0)
                        {
                            optionAnswer.AddRange(answerList);
                        }
                        tm.Set("OptionsAnswer", optionAnswer);
                    }
                    else
                    {
                        var optionsAnswers = new List<int>();
                        var optionsValues = new List<string>();
                        var options = ListUtils.ToList(tm.Get("options"));
                        if (options != null && options.Count > 0)
                        {
                            for (var i = 0; i < options.Count; i++)
                            {
                                optionsAnswers.Add(0);
                                var abcList = StringUtils.GetABC();
                                var optionAnswerValue = abcList[i];
                                optionsValues.Add(optionAnswerValue);
                                var answerCount = await _questionnaireAnswerRepository.GetCountSubmitUser(paper.Id, tm.Id, optionAnswerValue);
                                optionsAnswers[i] = answerCount;
                                tmAnswerToTal += answerCount;
                            }
                        }
                        tm.Set("OptionsAnswer", optionsAnswers);
                        tm.Set("OptionsValues", optionsValues);
                    }
                    tm.Set("TmAnswerTotal", tmAnswerToTal);
                }
            }

            paper.Set("TmTotal", tmTotal);
            return new GetResult
            {
                Item = paper,
                TmList = tmList
            };

        }
    }
}
