using Datory;
using System;
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
                    var allTrue = ExamUtils.IsAnswerAllTrue(answer, myAnswer, answerList);

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

            examPaperAnswer.Score = await ExecuteSubmitAnswerGetScoreAsync(examPaperAnswer.ExamPaperId, tm.TxId, tm.Score, examPaperAnswer.ExamTmType, tm.Answer, examPaperAnswer.Answer);

            await _databaseManager.ExamPaperAnswerRepository.UpdateAsync(examPaperAnswer);
        }

        public async Task ExecuteSubmitAnswerSmallAsync(ExamPaperAnswerSmall examPaperAnswer)
        {
            var tm = await _databaseManager.ExamPaperRandomTmSmallRepository.GetAsync(examPaperAnswer.RandomTmId);

            examPaperAnswer.Score = await ExecuteSubmitAnswerGetScoreAsync(examPaperAnswer.ExamPaperId, tm.TxId, tm.Score, examPaperAnswer.ExamTmType, tm.Answer, examPaperAnswer.Answer);

            await _databaseManager.ExamPaperAnswerSmallRepository.UpdateAsync(examPaperAnswer);

            decimal answerScore = 0;
            var smallAnswerList = await _databaseManager.ExamPaperAnswerSmallRepository.GetListAsync(examPaperAnswer.AnswerId);
            if (smallAnswerList != null && smallAnswerList.Count > 0)
            {
                foreach (var smallAnswer in smallAnswerList)
                {
                    answerScore += smallAnswer.Score;
                }
            }

            var answerInfo = await _databaseManager.ExamPaperAnswerRepository.GetAsync(examPaperAnswer.AnswerId, examPaperAnswer.ExamPaperId);
            answerInfo.Score = answerScore;
            await _databaseManager.ExamPaperAnswerRepository.UpdateAsync(answerInfo);
        }


        private async Task ExecuteSubmitPaperAsync(int startId)
        {
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

            start.IsPass = start.Score >= paper.PassScore;
            await _databaseManager.ExamPaperStartRepository.UpdateAsync(start);

            await _databaseManager.ExamPaperUserRepository.IncrementSubmitAsync(start.PlanId, start.CourseId, start.ExamPaperId, start.UserId);

            var user = await _databaseManager.UserRepository.GetByUserIdAsync(start.UserId);
            if (user != null)
            {
                await AddPointsLogAsync(PointType.PointExam, user, paper.Id, paper.Title);
            }


            if (start.IsMark && start.IsSubmit)
            {
                if (start.IsPass)
                {
                    await AddPointsLogAsync(PointType.PointExamPass, user, paper.Id, paper.Title);
                    if (start.Score >= paper.TotalScore)
                    {
                        await AddPointsLogAsync(PointType.PointExamFull, user, paper.Id, paper.Title);
                    }
                    if (paper.CerId > 0)
                    {
                        CreateExamAwardCerAsync(start.Id);
                    }
                }
            }
        }
        private async Task AddPointsLogAsync(PointType pointType, User user, int objectId = 0, string objectName = "")
        {
            var dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            var (value, maxvalue) = await _databaseManager.ConfigRepository.GetPointValueByPointType(pointType);
            var todayMaxvalue = await _databaseManager.PointLogRepository.GetSumPoint(pointType, user.Id, dateStr);

            var isNotice = pointType == PointType.PointExamFull || pointType == PointType.PointExamPass;
            if (maxvalue > todayMaxvalue)
            {
                var log = new PointLog()
                {
                    IsNotice = isNotice,
                    UserId = user.Id,
                    CompanyId = user.CompanyId,
                    DepartmentId = user.DepartmentId,
                    CompanyParentPath = user.CompanyParentPath,
                    DepartmentParentPath = user.DepartmentParentPath,
                    CreatorId = user.Id,
                    ObjectId = objectId,
                    ObjectName = objectName,
                    PointType = pointType,
                    DateStr = dateStr,
                    Point = value,
                    Subject = $"{pointType.GetDisplayName()}奖励积分"
                };
                await _databaseManager.PointLogRepository.InsertAsync(log);
                await _databaseManager.UserRepository.UpdatePointsAsync(value, user.Id);
            }
        }
    }
}
