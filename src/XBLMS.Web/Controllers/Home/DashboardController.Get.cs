using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home
{
    public partial class DashboardController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var user = await _authManager.GetUserAsync();

            if (user == null) return this.Error(Constants.ErrorNotFound);

            user = await _organManager.GetUser(user.Id);

            var (allPassPercent, allTotal, moniPassPercent, moniTotal, passPercent, total) = await _examManager.AnalysisMorePass(user.Id);
            var (answerTmTotal, answerPercent, allTmTotal, allPercent, collectTmTotal, collectPercent, wrongTmTotal, wrongPercent) = await _examManager.AnalysisPractice(user.Id);

            var resultPaper = new ExamPaper();
            var resultMoni = new ExamPaper();

            var paperIds = await _examPaperUserRepository.GetPaperIdsByUser(user.Id, "");
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
                PracticeWrongPercent = wrongPercent
            };
        }
    }
}
