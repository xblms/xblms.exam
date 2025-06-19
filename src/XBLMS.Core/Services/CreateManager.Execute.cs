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
        public async Task<decimal> ExecuteSubmitAnswerGetScoreAsync(int paperId, int txId, decimal tmScore, ExamTmType examTmType, string answer, string myAnswer)
        {
            decimal score = 0;
            if (examTmType == ExamTmType.Objective)
            {
                if (StringUtils.EqualsIgnoreCase(answer, myAnswer))
                {
                    score = tmScore;
                }
                else
                {
                    decimal answerTotalScore = 0;
                    var paper = await _databaseManager.ExamPaperRepository.GetAsync(paperId);
                    var tx = await _databaseManager.ExamTxRepository.GetAsync(txId);
                    if (tx.ExamTxBase == ExamTxBase.Duoxuanti && paper.IsAutoScoreDuoxuanti && tmScore > 0)
                    {
                        var answerlist = ListUtils.ToList(answer.ToCharArray());
                        if (answerlist.Count > 1)
                        {
                            var myAnswerList = ListUtils.ToList(myAnswer);
                            var itemScore = Math.Round(tmScore / answerlist.Count, 2);

                            foreach (var myanswer in myAnswerList)
                            {
                                if (StringUtils.ContainsIgnoreCase(answer, myanswer))
                                {
                                    answerTotalScore += itemScore;
                                }
                            }
                        }
                    }
                    score = answerTotalScore;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(answer))
                {
                    var answerList = ListUtils.GetStringList(answer);
                    var allTrue = true;
                    foreach (var answerItem in answerList)
                    {
                        if (!StringUtils.ContainsIgnoreCase(myAnswer, answerItem))
                        {
                            allTrue = false;
                        }
                    }
                    if (StringUtils.ContainsIgnoreCase(answer, ";") && !allTrue)
                    {
                        answerList = ListUtils.GetStringList(answer, ";");
                        foreach (var answerItem in answerList)
                        {
                            if (!StringUtils.ContainsIgnoreCase(myAnswer, answerItem))
                            {
                                allTrue = false;
                            }
                        }
                    }
                    if (StringUtils.ContainsIgnoreCase(answer, "，") && !allTrue)
                    {
                        answerList = ListUtils.GetStringList(answer, "，");
                        foreach (var answerItem in answerList)
                        {
                            if (!StringUtils.ContainsIgnoreCase(myAnswer, answerItem))
                            {
                                allTrue = false;
                            }
                        }
                    }
                    if (StringUtils.ContainsIgnoreCase(answer, "；") && !allTrue)
                    {
                        answerList = ListUtils.GetStringList(answer, "；");
                        foreach (var answerItem in answerList)
                        {
                            if (!StringUtils.ContainsIgnoreCase(myAnswer, answerItem))
                            {
                                allTrue = false;
                            }
                        }
                    }

                    if (allTrue)
                    {
                        score = tmScore;
                    }
                    else
                    {
                        if (answerList.Count > 1)
                        {
                            decimal answerTotalScore = 0;
                            var paper = await _databaseManager.ExamPaperRepository.GetAsync(paperId);
                            var tx = await _databaseManager.ExamTxRepository.GetAsync(txId);
                            if (tx.ExamTxBase == ExamTxBase.Tiankongti && paper.IsAutoScoreTiankongti && tmScore > 0)
                            {
                                var myAnswerList = ListUtils.GetStringList(myAnswer);
                                var itemScore = Math.Round(tmScore / answerList.Count, 2);
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
                            if (tx.ExamTxBase == ExamTxBase.Jiandati && paper.IsAutoScoreJiandati && tmScore > 0)
                            {
                                var itemScore = Math.Round(tmScore / answerList.Count, 2);
                                foreach (var answerItem in answerList)
                                {
                                    if (StringUtils.ContainsIgnoreCase(myAnswer, answerItem))
                                    {
                                        answerTotalScore += itemScore;
                                    }
                                }
                            }
                            score = answerTotalScore;
                        }

                    }
                }
            }
            return score;
        }
        public async Task ExecuteSubmitAnswerAsync(ExamPaperAnswer examPaperAnswer)
        {
            var tm = await _databaseManager.ExamPaperRandomTmRepository.GetAsync(examPaperAnswer.RandomTmId, examPaperAnswer.ExamPaperId);

            examPaperAnswer.Score= await ExecuteSubmitAnswerGetScoreAsync(examPaperAnswer.ExamPaperId, tm.TxId, tm.Score, examPaperAnswer.ExamTmType, tm.Answer, examPaperAnswer.Answer);

            await _databaseManager.ExamPaperAnswerRepository.UpdateAsync(examPaperAnswer);
        }

        public async Task ExecuteSubmitAnswerSmallAsync(ExamPaperAnswerSmall examPaperAnswer)
        {
            var tm = await _databaseManager.ExamPaperRandomTmSmallRepository.GetAsync(examPaperAnswer.RandomTmId);

            examPaperAnswer.Score = await ExecuteSubmitAnswerGetScoreAsync(examPaperAnswer.ExamPaperId, tm.TxId, tm.Score, examPaperAnswer.ExamTmType, tm.Answer, examPaperAnswer.Answer);

            await _databaseManager.ExamPaperAnswerSmallRepository.UpdateAsync(examPaperAnswer);

            decimal answerScore = 0;
            var smallAnswerList = await _databaseManager.ExamPaperAnswerSmallRepository.GetListAsync(examPaperAnswer.AnswerId);
            if(smallAnswerList!=null && smallAnswerList.Count > 0)
            {
                foreach (var smallAnswer in smallAnswerList) {
                    answerScore += smallAnswer.Score;
                }
            }

            var answerInfo = await _databaseManager.ExamPaperAnswerRepository.GetAsync(examPaperAnswer.AnswerId, examPaperAnswer.ExamPaperId);
            answerInfo.Score = answerScore;
            await _databaseManager.ExamPaperAnswerRepository.UpdateAsync(answerInfo);
        }



        public async Task ExecuteSubmitPaperAsync(int startId)
        {
            //Thread.Sleep(1000 * 10);
            var start = await _databaseManager.ExamPaperStartRepository.GetAsync(startId);
            var paper = await _databaseManager.ExamPaperRepository.GetAsync(start.ExamPaperId);


            if (start.HaveSubjective)
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
