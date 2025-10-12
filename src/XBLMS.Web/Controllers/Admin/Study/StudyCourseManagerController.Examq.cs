using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseManagerController
    {
        [HttpGet, Route(RouteQ)]
        public async Task<ActionResult<GetExamqResult>> GetCourse([FromQuery] GetExamqRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }

            var course = await _studyCourseRepository.GetAsync(request.Id);
            if (request.PlanId > 0)
            {
                var planCourse = await _studyPlanCourseRepository.GetAsync(request.PlanId, request.Id);
                course.ExamQuestionnaireId = planCourse.ExamQuestionnaireId;
            }

            var tmTotal = 0;
            var answerTotal = 0;
            var resutlList = new List<ExamQuestionnaireTm>();


            if (course.ExamQuestionnaireId > 0)
            {
                var paper = await _examQuestionnaireRepository.GetAsync(course.ExamQuestionnaireId);
                answerTotal = await _examQuestionnaireUserRepository.GetSubmitUserCountAsync(request.PlanId, request.Id, paper.Id);

                if (paper != null)
                {
                    var tmList = await _examQuestionnaireTmRepository.GetListAsync(paper.Id);

                    if (tmList != null && tmList.Count > 0)
                    {
                        foreach (var tm in tmList)
                        {
                            tmTotal++;
                            var tmAnswerToTal = 0;
                            var optionAnswer = new List<string>();
                            if (tm.Tx == ExamQuestionnaireTxType.Jiandati)
                            {
                                var answerList = await _examQuestionnaireAnswerRepository.GetListAnswer(request.PlanId, request.Id, paper.Id, tm.Id);
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
                                        var answerCount = await _examQuestionnaireAnswerRepository.GetCountSubmitUser(request.PlanId, request.Id, paper.Id, tm.Id, optionAnswerValue);
                                        optionsAnswers[i] = answerCount;
                                        tmAnswerToTal += answerCount;
                                    }
                                }
                                tm.Set("OptionsAnswer", optionsAnswers);
                                tm.Set("OptionsValues", optionsValues);
                            }
                            tm.Set("TmAnswerTotal", tmAnswerToTal);
                            resutlList.Add(tm);
                        }
                    }
                }
        
            }

            return new GetExamqResult
            {
                QTmTotal = tmTotal,
                QAnswerTotal = answerTotal,
                QList = resutlList
            };
        }
    }
}
