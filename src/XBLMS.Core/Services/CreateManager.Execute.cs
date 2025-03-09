using System;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class CreateManager
    {
        public async Task ExecuteSubmitAnswerAsync(ExamPaperAnswer examPaperAnswer)
        {
            var tm = await _databaseManager.ExamPaperRandomTmRepository.GetAsync(examPaperAnswer.RandomTmId, examPaperAnswer.ExamPaperId);
            if (examPaperAnswer.ExamTmType == ExamTmType.Objective)
            {
                if (StringUtils.Equals(tm.Answer, examPaperAnswer.Answer))
                {
                    examPaperAnswer.Score = tm.Score;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(tm.Answer))
                {
                    var answerList = ListUtils.GetStringList(tm.Answer);
                    var allTrue = true;
                    foreach (var answer in answerList)
                    {
                        if (!StringUtils.Contains(examPaperAnswer.Answer, answer))
                        {
                            allTrue = false;
                        }
                    }
                    if (!allTrue)
                    {
                        answerList = ListUtils.GetStringList(tm.Answer, ";");
                        foreach (var answer in answerList)
                        {
                            if (!StringUtils.Contains(examPaperAnswer.Answer, answer))
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
                            if (!StringUtils.Contains(examPaperAnswer.Answer, answer))
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
                            if (!StringUtils.Contains(examPaperAnswer.Answer, answer))
                            {
                                allTrue = false;
                            }
                        }
                    }
                    if (allTrue)
                    {
                        examPaperAnswer.Score = tm.Score;
                    }
                }
            }

            await _databaseManager.ExamPaperAnswerRepository.UpdateAsync(examPaperAnswer);
        }
        public async Task ExecuteSubmitPaperAsync(int startId)
        {
            //Thread.Sleep(1000 * 10);
            var start = await _databaseManager.ExamPaperStartRepository.GetAsync(startId);
            var paper = await _databaseManager.ExamPaperRepository.GetAsync(start.ExamPaperId);

            bool hasZhuguanti = false;
            var txList = await _databaseManager.ExamTxRepository.GetListAsync();
            if (txList != null && txList.Count > 0 && paper.TxIds != null && paper.TxIds.Count > 0)
            {
                txList = txList.Where(tx => paper.TxIds.Contains(tx.Id)).ToList();
                if (txList != null && txList.Count > 0)
                {
                    foreach (var item in txList)
                    {
                        if (item.ExamTxBase == ExamTxBase.Tiankongti || item.ExamTxBase == ExamTxBase.Jiandati)
                        {
                            hasZhuguanti = true;
                            break;
                        }
                    }
                }
            }
            if (hasZhuguanti)
            {
                if (paper.IsAutoScore)
                {
                    start.IsMark = true;
                }
                else
                {
                    start.IsMark = false;
                }
            }
            else
            {
                start.IsMark = true;
            }

            start.IsSubmit = true;
            start.EndDateTime = DateTime.Now;

            var sumScore = await _databaseManager.ExamPaperAnswerRepository.ScoreSumAsync(startId, paper.Id);
            var objectiveSocre = await _databaseManager.ExamPaperAnswerRepository.ObjectiveScoreSumAsync(startId, paper.Id);
            var subjectiveScore = await _databaseManager.ExamPaperAnswerRepository.SubjectiveScoreSumAsync(startId, paper.Id);

            start.ObjectiveScore = objectiveSocre;

            if (start.IsMark)
            {
                start.SubjectiveScore = subjectiveScore;
                start.Score = sumScore;
            }
            else
            {
                start.SubjectiveScore = 0;
                start.Score = objectiveSocre;
            }

            await _databaseManager.ExamPaperStartRepository.UpdateAsync(start);

            if (start.IsMark && start.IsSubmit && start.Score >= paper.PassScore && paper.CerId > 0)
            {
                await AwardCer(paper, startId, start.UserId);
            }
        }
    }
}
