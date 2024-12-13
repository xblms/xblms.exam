using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmAnalysisController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest reqeust)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var analysis = await _examTmAnalysisRepository.GetAsync(reqeust.Type, reqeust.PaperId);
            var total = 0;
            var list = new List<ExamTmAnalysisTm>();
            if (analysis != null)
            {
                if (reqeust.ReAnalysis)
                {
                    await _examTmAnalysisTmRepository.DeleteAsync(analysis.Id);
                    analysis.TmAnalysisDateTime = DateTime.Now;
                    await _examTmAnalysisRepository.UpdateAsync(analysis);
                    await Analysis(analysis);
                }
            }
            else
            {
                var analysisId = await _examTmAnalysisRepository.InsertAsync(new ExamTmAnalysis
                {
                    TmAnalysisDateTime = DateTime.Now,
                    TmAnalysisExamPapaerId = reqeust.PaperId,
                    TmAnalysisType = reqeust.Type
                });
                analysis = await _examTmAnalysisRepository.GetAsync(reqeust.Type, reqeust.PaperId);
                await Analysis(analysis);
            }

            (total, list) = await _examTmAnalysisTmRepository.GetListAsync(reqeust.OrderType, reqeust.KeyWords, analysis.Id, reqeust.PageIndex, reqeust.PageSize);

            return new GetResult
            {
                Total = total,
                List = list,
                PId = analysis.Id,
                PDate = analysis.TmAnalysisDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN)
            };
        }
        private async Task Analysis(ExamTmAnalysis analysis)
        {

            if (analysis.TmAnalysisType == TmAnalysisType.ByExamAll)
            {
                var tmIds = await _examTmRepository.GetIdsWithOutLockedAsync();
                foreach (var tmId in tmIds)
                {
                    var tm = await _examTmRepository.GetAsync(tmId);
                    var randomIds = await _examPaperRandomTmRepository.GetIdsAsync(tmId);
                    var rightTotal = 0;
                    var wrongTotal = 0;
                    var answerTotal = 0;
                    if (randomIds != null && randomIds.Count > 0)
                    {
                        foreach (var randomId in randomIds)
                        {
                            var (rightCount, wrongCount) = await _examPaperAnswerRepository.CountAsync(randomId, analysis.TmAnalysisExamPapaerId);
                            rightTotal += rightCount;
                            wrongTotal += wrongCount;
                        }
                    }
                    answerTotal = rightTotal + wrongTotal;
                    var errorPercent = 0.00;
                    var errorRate = 0.00;
                    if (wrongTotal > 0)
                    {
                        errorPercent = wrongTotal * 100 / answerTotal;
                        errorRate = ((double)answerTotal / ((double)answerTotal + 5)) * (double)wrongTotal;
                    }
                    var analysisTm = new ExamTmAnalysisTm
                    {
                        TmId = tm.Id,
                        AnalysisId = analysis.Id,
                        AnswerCount = answerTotal,
                        RightCount = rightTotal,
                        WrongCount = wrongTotal,
                        WrongRate = TranslateUtils.ToDecimal(errorRate.ToString(), 2),
                        WrongPercent = TranslateUtils.ToDecimal(errorPercent.ToString(), 2),
                        Title = StringUtils.StripTags(tm.Title),
                        TxId = tm.TxId,
                        TreeId = tm.TreeId,
                        Jiexi = tm.Jiexi,
                        Zhishidian = tm.Zhishidian,
                        Nandu = tm.Nandu,
                        Score = tm.Score,
                    };
                    await _examTmAnalysisTmRepository.InsertAsync(analysisTm);
                }
            }
            if (analysis.TmAnalysisType == TmAnalysisType.ByPractice)
            {
                var tmIds = await _examTmRepository.GetIdsWithOutLockedAsync();
                foreach (var tmId in tmIds)
                {
                    var tm = await _examTmRepository.GetAsync(tmId);

                    var (rightTotal, wrongTotal) = await _examPracticeAnswerRepository.CountAsync(tmId);

                    var answerTotal = rightTotal + wrongTotal;
                    if (answerTotal > 0)
                    {
                        var errorPercent = 0.00;
                        var errorRate = 0.00;
                        if (wrongTotal > 0)
                        {
                            errorPercent = wrongTotal * 100 / answerTotal;
                            errorRate = ((double)answerTotal / ((double)answerTotal + 5)) * (double)wrongTotal;
                        }
                        var analysisTm = new ExamTmAnalysisTm
                        {
                            TmId = tm.Id,
                            AnalysisId = analysis.Id,
                            AnswerCount = answerTotal,
                            RightCount = rightTotal,
                            WrongCount = wrongTotal,
                            WrongRate = TranslateUtils.ToDecimal(errorRate.ToString(), 2),
                            WrongPercent = TranslateUtils.ToDecimal(errorPercent.ToString(), 2),
                            Title = StringUtils.StripTags(tm.Title),
                            TxId = tm.TxId,
                            TreeId = tm.TreeId,
                            Jiexi = tm.Jiexi,
                            Zhishidian = tm.Zhishidian,
                            Nandu = tm.Nandu,
                            Score = tm.Score,
                        };
                        await _examTmAnalysisTmRepository.InsertAsync(analysisTm);
                    }


                }
            }
            if (analysis.TmAnalysisType == TmAnalysisType.ByExamOnlyOne)
            {
                var tmIds = await _examTmRepository.GetIdsWithOutLockedAsync();
                foreach (var tmId in tmIds)
                {
                    var randomIds = await _examPaperRandomTmRepository.GetIdsAsync(tmId);
                    var rightTotal = 0;
                    var wrongTotal = 0;
                    var answerTotal = 0;
                    if (randomIds != null && randomIds.Count > 0)
                    {
                        var tm = await _examTmRepository.GetAsync(tmId);

                        foreach (var randomId in randomIds)
                        {
                            var (rightCount, wrongCount) = await _examPaperAnswerRepository.CountAsync(randomId, analysis.TmAnalysisExamPapaerId);
                            rightTotal += rightCount;
                            wrongTotal += wrongCount;
                        }

                        answerTotal = rightTotal + wrongTotal;
                        var errorPercent = 0.00;
                        var errorRate = 0.00;
                        if (wrongTotal > 0)
                        {
                            errorPercent = wrongTotal * 100 / answerTotal;
                            errorRate = ((double)answerTotal / ((double)answerTotal + 5)) * (double)wrongTotal;
                        }
                        var analysisTm = new ExamTmAnalysisTm
                        {
                            TmId = tm.Id,
                            AnalysisId = analysis.Id,
                            AnswerCount = answerTotal,
                            RightCount = rightTotal,
                            WrongCount = wrongTotal,
                            WrongRate = TranslateUtils.ToDecimal(errorRate.ToString(), 2),
                            WrongPercent = TranslateUtils.ToDecimal(errorPercent.ToString(), 2),
                            Title = StringUtils.StripTags(tm.Title),
                            TxId = tm.TxId,
                            TreeId = tm.TreeId,
                            Jiexi = tm.Jiexi,
                            Zhishidian = tm.Zhishidian,
                            Nandu = tm.Nandu,
                            Score = tm.Score,
                        };
                        await _examTmAnalysisTmRepository.InsertAsync(analysisTm);
                    }

                }
            }
        }
    }
}
