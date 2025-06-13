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
            examPaperAnswer.Score = 0;

            var tm = await _databaseManager.ExamPaperRandomTmRepository.GetAsync(examPaperAnswer.RandomTmId, examPaperAnswer.ExamPaperId);
            if (examPaperAnswer.ExamTmType == ExamTmType.Objective)
            {
                if (StringUtils.EqualsIgnoreCase(tm.Answer, examPaperAnswer.Answer))
                {
                    examPaperAnswer.Score = tm.Score;
                }
                else
                {
                    decimal answerTotalScore = 0;
                    var paper = await _databaseManager.ExamPaperRepository.GetAsync(examPaperAnswer.ExamPaperId);
                    var tx = await _databaseManager.ExamTxRepository.GetAsync(tm.TxId);
                    if (tx.ExamTxBase == ExamTxBase.Duoxuanti && paper.IsAutoScoreDuoxuanti && tm.Score > 0)
                    {
                        var answerlist = ListUtils.ToList(tm.Answer.ToCharArray());
                        if (answerlist.Count > 1)
                        {
                            var myAnswerList = ListUtils.ToList(examPaperAnswer.Answer.ToCharArray());
                            var itemScore = Math.Round(tm.Score / answerlist.Count, 2);

                            foreach (var myanswer in myAnswerList)
                            {
                                if (StringUtils.ContainsIgnoreCase(tm.Answer, myanswer))
                                {
                                    answerTotalScore += itemScore;
                                }
                            }
                        }
                    }
                    examPaperAnswer.Score = answerTotalScore;
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
                        if (!StringUtils.ContainsIgnoreCase(examPaperAnswer.Answer, answer))
                        {
                            allTrue = false;
                        }
                    }
                    if (StringUtils.ContainsIgnoreCase(tm.Answer, ";") && !allTrue)
                    {
                        answerList = ListUtils.GetStringList(tm.Answer, ";");
                        foreach (var answer in answerList)
                        {
                            if (!StringUtils.ContainsIgnoreCase(examPaperAnswer.Answer, answer))
                            {
                                allTrue = false;
                            }
                        }
                    }
                    if (StringUtils.ContainsIgnoreCase(tm.Answer, "，") && !allTrue)
                    {
                        answerList = ListUtils.GetStringList(tm.Answer, "，");
                        foreach (var answer in answerList)
                        {
                            if (!StringUtils.ContainsIgnoreCase(examPaperAnswer.Answer, answer))
                            {
                                allTrue = false;
                            }
                        }
                    }
                    if (StringUtils.ContainsIgnoreCase(tm.Answer, "；") && !allTrue)
                    {
                        answerList = ListUtils.GetStringList(tm.Answer, "；");
                        foreach (var answer in answerList)
                        {
                            if (!StringUtils.ContainsIgnoreCase(examPaperAnswer.Answer, answer))
                            {
                                allTrue = false;
                            }
                        }
                    }

                    if (allTrue)
                    {
                        examPaperAnswer.Score = tm.Score;
                    }
                    else
                    {
                        if (answerList.Count > 1)
                        {
                            decimal answerTotalScore = 0;
                            var paper = await _databaseManager.ExamPaperRepository.GetAsync(examPaperAnswer.ExamPaperId);
                            var tx = await _databaseManager.ExamTxRepository.GetAsync(tm.TxId);
                            if (tx.ExamTxBase == ExamTxBase.Tiankongti && paper.IsAutoScoreTiankongti && tm.Score > 0)
                            {
                                var myAnswerList = ListUtils.GetStringList(examPaperAnswer.Answer);
                                var itemScore = Math.Round(tm.Score / answerList.Count, 2);
                                if (answerList.Count >= myAnswerList.Count)
                                {
                                    for (var i = 0; i < myAnswerList.Count; i++)
                                    {
                                        if (StringUtils.EqualsIgnoreCase(answerList[i], myAnswerList[i]))
                                        {
                                            answerTotalScore += itemScore;
                                        }
                                    }
                                }
                            }
                            if (tx.ExamTxBase == ExamTxBase.Jiandati && paper.IsAutoScoreJiandati && tm.Score > 0)
                            {
                                var itemScore = Math.Round(tm.Score / answerList.Count, 2);
                                foreach(var answer in answerList)
                                {
                                    if (StringUtils.ContainsIgnoreCase(examPaperAnswer.Answer, answer))
                                    {
                                        answerTotalScore += itemScore;
                                    }
                                }
                            }
                            examPaperAnswer.Score = answerTotalScore;
                        }

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
