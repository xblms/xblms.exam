using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;

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

            var paperIds = await _examPaperUserRepository.GetPaperIdsByUser(user.Id, request.IsApp);
            var (paperTotal, paperList) = await _examPaperRepository.GetListByUserAsync(paperIds, "", 1, 1);
            var (moniPaperTotal, moniPaperList) = await _examPaperRepository.GetListByUserAsync(paperIds, "", 1, 1, true);

            if (paperTotal > 0)
            {
                resultPaper = paperList[0];
                await _examManager.GetPaperInfo(resultPaper, user);
            }
            else
            {
                resultPaper = null;
            }
            if (moniPaperTotal > 0)
            {
                resultMoni = moniPaperList[0];
                await _examManager.GetPaperInfo(resultMoni, user);
            }
            else
            {
                resultMoni = null;
            }



            var taskPaperTotal = 0;

            var taskPaperIds = paperIds;
            if (taskPaperIds != null && taskPaperIds.Count > 0)
            {
                foreach (var paperId in taskPaperIds)
                {
                    var paper = await _examPaperRepository.GetAsync(paperId);
                    var myExamTimes = await _examPaperStartRepository.CountAsync(paperId, user.Id);
                    if (myExamTimes <= 0 && (paper.ExamBeginDateTime.Value < DateTime.Now && paper.ExamEndDateTime.Value > DateTime.Now))
                    {
                        if (paper.Moni)
                        {

                        }
                        else
                        {
                            taskPaperTotal++;
                        }
                    }
                }

            }
            var qPaperTotal = await _examQuestionnaireUserRepository.GetTaskCountAsync(user.Id);
            var assesstantTask = await _examAssessmentUserRepository.GetTaskCountAsync(user.Id);

            var topCer = new ExamCerUser();
            var (cerTotal, cerList) = await _examCerUserRepository.GetListAsync(user.Id, 1, 1);
            if (total > 0)
            {
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
                    topCer = item;
                }
            }


            var dateStr = $"{DateTime.Now.ToString(DateUtils.FormatStringDateOnlyCN)} {DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-CN"))}";

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
                TaskAssTotal = assesstantTask,

                TopCer = topCer,
                DateStr = dateStr,
            };
        }
    }
}
