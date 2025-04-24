using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home
{
    public partial class DashboardController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            user = await _organManager.GetUser(user.Id);

            var (allPassPercent, allTotal, moniPassPercent, moniTotal, passPercent, total) = await _examManager.AnalysisMorePass(user.Id);
            var (answerTmTotal, answerPercent, allTmTotal, allPercent, collectTmTotal, collectPercent, wrongTmTotal, wrongPercent) = await _examManager.AnalysisPractice(user.Id);

            var resultPaper = new ExamPaper();
            var resultMoni = new ExamPaper();

            var (paperTotal, paperList) = await _examPaperUserRepository.GetListAsync(user.Id, false, request.IsApp, "", "", 1, 1);
            var (moniPaperTotal, moniPaperList) = await _examPaperUserRepository.GetListAsync(user.Id, true, request.IsApp, "", "", 1, 1);

            if (paperTotal > 0)
            {
                var paperUser = paperList[0];
                var paper = await _examPaperRepository.GetAsync(paperUser.ExamPaperId);
                await _examManager.GetPaperInfo(paper, user);
                resultPaper = paper;
            }
            else
            {
                resultPaper = null;
            }
            if (moniPaperTotal > 0)
            {
                var paperUser = moniPaperList[0];
                var paper = await _examPaperRepository.GetAsync(paperUser.ExamPaperId);
                await _examManager.GetPaperInfo(paper, user);
                resultMoni = paper;
            }
            else
            {
                resultMoni = null;
            }

            var openMenus = await _userMenuRepository.GetOpenMenusAsync();

            var taskTotal = 0;
            var taskPaperTotal = 0;
            var taskPaperList = new List<ExamPaper>();
            var todayExam = new ExamPaper();
            var taskPaperIds = await _examPaperUserRepository.GetPaperIdsByUser(user.Id);
            if (taskPaperIds != null && taskPaperIds.Count > 0 && ListUtils.Contains(openMenus, "examPaper"))
            {
                foreach (var paperId in taskPaperIds)
                {
                    var paper = await _examPaperRepository.GetAsync(paperId);
                    if (paper != null)
                    {
                        var myExamTimes = await _examPaperStartRepository.CountAsync(paperId, user.Id);
                        if ((paper.ExamBeginDateTime.Value < DateTime.Now && paper.ExamEndDateTime.Value > DateTime.Now))
                        {
                            if (!paper.Moni)
                            {
                                await _examManager.GetPaperInfo(paper, user);
                                if (myExamTimes <= 0)
                                {
                                    taskPaperTotal++;
                                    taskTotal++;
                                    taskPaperList.Add(paper);
                                }
                                if (paper.ExamBeginDateTime.Value.Date == DateTime.Now.Date && todayExam.Id == 0)
                                {
                                    todayExam = paper;
                                }
                            }
                        }
                    }
                }
            }
            var taskQList = new List<ExamQuestionnaire>();
            var (qPaperTotal, qPaperList) = await _examQuestionnaireUserRepository.GetTaskAsync(user.Id);
            if (qPaperTotal > 0 && ListUtils.Contains(openMenus, "examQuestionnaire"))
            {
                foreach (var item in qPaperList)
                {
                    var paper = await _examQuestionnaireRepository.GetAsync(item.ExamPaperId);
                    await _examManager.GetQuestionnaireInfo(paper, user);
                    taskQList.Add(paper);
                    taskTotal++;
                }
            }

            var taskAssList = new List<ExamAssessment>();
            var (assesstantTaskTotal, assesstantTaskList) = await _examAssessmentUserRepository.GetTaskAsync(user.Id);
            if (assesstantTaskTotal > 0 && ListUtils.Contains(openMenus, "examAssessment"))
            {
                foreach (var item in assesstantTaskList)
                {
                    var assInfo = await _examAssessmentRepository.GetAsync(item.ExamAssId);
                    _examManager.GetExamAssessmentInfo(assInfo, item, user);
                    taskTotal++;
                    taskAssList.Add(assInfo);
                }
            }

            var topCer = new ExamCerUser();
            var (cerTotal, cerList) = await _examCerUserRepository.GetListAsync(user.Id, 1, 8);
            if (cerTotal > 0 && ListUtils.Contains(openMenus, "examPaperCer"))
            {
                var cerIndex = 0;
                foreach (var item in cerList)
                {
                    var cerInfo = await _examCerRepository.GetAsync(item.CerId);
                    if (cerInfo != null)
                    {
                        item.Set("CerName", cerInfo.Name);
                        item.Set("CerOrganName", cerInfo.OrganName);
                    }
                    else
                    {
                        item.Set("CerName", "证书异常");
                    }
                    item.Set("AwartDate", item.CerDateTime.Value.ToString(DateUtils.FormatStringDateOnlyCN));
                    var paper = await _examPaperRepository.GetAsync(item.ExamPaperId);
                    if (paper != null)
                    {
                        item.Set("PaperName", paper.Title);
                    }
                    else
                    {
                        item.Set("PaperName", "试卷异常");
                    }
                    var start = await _examPaperStartRepository.GetAsync(item.ExamStartId);
                    if (start != null)
                    {
                        item.Set("PaperScore", start.Score);
                    }
                    else
                    {
                        item.Set("PaperScore", "成绩异常");
                    }
                    if (cerIndex == 0)
                    {
                        topCer = item;
                    }
                    cerIndex++;
                }
            }

            var dateStr = $"{DateTime.Now.ToString(DateUtils.FormatStringDateOnlyCN)} {DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-CN"))}";

            var knowList = await _knowlegesRepository.GetNewListAsync();
            if(knowList!=null && knowList.Count > 0)
            {
                foreach (var item in knowList)
                {
                    item.Url = "";
                }
            }
            return new GetResult
            {
                User = user,
                AllPercent = allPassPercent,
                ExamTotal = total,
                ExamPercent = allPassPercent,
                ExamMoniPercent = moniPassPercent,
                ExamMoniTotal = moniTotal,

                ExamPaper = resultPaper,
                ExamMoni = resultMoni,

                PracticeAnswerTmTotal = answerTmTotal,
                PracticeAnswerPercent = answerPercent,
                PracticeAllTmTotal = allTmTotal,
                PracticeAllPercent = allPercent,
                PracticeCollectTmTotal = collectTmTotal,
                PracticeCollectPercent = collectPercent,
                PracticeWrongTmTotal = wrongTmTotal,
                PracticeWrongPercent = wrongPercent,

                TaskPaperTotal = taskPaperTotal,
                TaskQTotal = qPaperTotal,
                TaskAssTotal = assesstantTaskTotal,

                TopCer = topCer,
                DateStr = dateStr,

                CerList = cerList,
                KnowList = knowList,
                TodayExam = todayExam,
                TaskPaperList = taskPaperList,
                TaskQList = taskQList,
                TaskAssList = taskAssList,
                TaskTotal = taskTotal,

                OpenMenus = openMenus,

                Version = _settingsManager.Version
            };
        }
    }
}
