using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task<bool> PaperRandomSet(ExamPaper paper)
        {
            if (paper.TmRandomType == ExamPaperTmRandomType.RandomNone)
            {
                #region 固定出题

                await _examPaperRandomConfigRepository.DeleteByPaperAsync(paper.Id);

                var paperRandom = new ExamPaperRandom()
                {
                    ExamPaperId = paper.Id,
                };

                var txIds = new List<int>();
                var randomId = await _examPaperRandomRepository.InsertAsync(paperRandom);
                if (randomId > 0)
                {
                    var tmIds = new List<int>();
                    var tmGroupIds = paper.TmGroupIds;
                    if (tmGroupIds != null && tmGroupIds.Count > 0)
                    {
                        foreach (var tmGroupId in tmGroupIds)
                        {
                            var tmGroup = await _examTmGroupRepository.GetAsync(tmGroupId);
                            if (tmGroup != null && tmGroup.TmIds != null && tmGroup.TmIds.Count > 0)
                            {
                                tmIds.AddRange(tmGroup.TmIds);
                            }
                        }

                    }
                    if (tmIds.Count > 0)
                    {
                        tmIds = tmIds.Distinct().ToList();
                        var tmList = await _examTmRepository.GetListWithOutLockedAsync(tmIds);
                        if (tmList != null && tmList.Count > 0)
                        {
                            txIds = tmList.Select(tm => tm.TxId).ToList();
                            await SetExamPaperRandomTm(tmList, paper, randomId);

                        }
                    }

                }

                if (txIds.Count > 0)
                {
                    txIds = txIds.Distinct().ToList();
                    var txs = new List<ExamTx>();
                    foreach (var txId in txIds)
                    {
                        var tx = await _examTxRepository.GetAsync(txId);
                        if (tx != null)
                        {
                            txs.Add(tx);
                        }
                    }
                    txs = txs.OrderBy(tx => tx.Taxis).ToList();
                    if (txs.Count > 0)
                    {
                        foreach (var tx in txs)
                        {
                            var tmList = await _examPaperRandomTmRepository.GetListAsync(randomId, tx.Id, paper.Id);
                            var tmTotal = 0;
                            decimal scoreTotal = 0;
                            var tmIds = new List<int>();
                            if (tmList != null && tmList.Count > 0)
                            {
                                tmTotal = tmList.Count;
                                scoreTotal = tmList.Sum(tm => tm.Score);
                                tmIds = tmList.Select(tm => tm.Id).ToList();
                            }
                            await _examPaperRandomConfigRepository.InsertAsync(new ExamPaperRandomConfig
                            {
                                ExamPaperId = paper.Id,
                                TxId = tx.Id,
                                TxName = tx.Name,
                                ScoreTotal = scoreTotal,
                                TmTotal = tmTotal,
                                TmIds = tmIds,
                                TxTaxis = tx.Taxis,
                            });
                        }
                    }
                    paper.TxIds = txIds;
                }

                #endregion
            }
            if (paper.TmRandomType == ExamPaperTmRandomType.RandomNow)
            {
                await SetExamPaperRantomByRandomNowAndExaming(paper);
            }
            return true;
        }
        public async Task SetExamPaperRantomByRandomNowAndExaming(ExamPaper paper, bool isExaming = false)
        {
            if (isExaming)
            {
                paper.RandomCount = 1;
            }
            var tmIds = new List<int>();
            var tmGroupIds = paper.TmGroupIds;
            var hasTmGroup = false;
            var allTm = false;
            if (tmGroupIds != null && tmGroupIds.Count > 0)
            {
                hasTmGroup = true;
                foreach (var tmGroupId in tmGroupIds)
                {
                    var tmGroup = await _examTmGroupRepository.GetAsync(tmGroupId);
                    if (tmGroup != null)
                    {
                        if (tmGroup.GroupType == TmGroupType.Fixed && tmGroup.TmIds != null && tmGroup.TmIds.Count > 0)
                        {
                            if (tmGroup.TmIds != null && tmGroup.TmIds.Count > 0)
                            {
                                tmIds.AddRange(tmGroup.TmIds);
                            }

                        }
                        if (tmGroup.GroupType == TmGroupType.Range)
                        {
                            var tmIdsByGroup = await _examTmRepository.Group_RangeIdsAsync(tmGroup);
                            if (tmIdsByGroup != null && tmIdsByGroup.Count > 0)
                            {
                                tmIds.AddRange(tmIdsByGroup);
                            }
                        }
                        if (tmGroup.GroupType == TmGroupType.All)
                        {
                            allTm = true;
                        }
                    }
                }

            }
            else
            {
                allTm = true;
            }

            if (paper.RandomCount > 0)
            {
                for (var i = 1; i <= paper.RandomCount; i++)
                {
                    var paperRandom = new ExamPaperRandom()
                    {
                        ExamPaperId = paper.Id,
                    };
                    var randomId = await _examPaperRandomRepository.InsertAsync(paperRandom);
                    var configList = await _examPaperRandomConfigRepository.GetListAsync(paper.Id);
                    if (configList != null && configList.Count > 0)
                    {
                        var tms = new List<ExamTm>();
                        foreach (var config in configList)
                        {
                            var tmList = await _examTmRepository.GetListByRandomAsync(allTm, hasTmGroup, tmIds, config.TxId, config.Nandu1TmCount, 0, 0, 0, 0);
                            if (tmList != null && tmList.Count > 0)
                            {
                                tms.AddRange(tmList);
                            }
                            tmList = await _examTmRepository.GetListByRandomAsync(allTm, hasTmGroup, tmIds, config.TxId, 0, config.Nandu2TmCount, 0, 0, 0);
                            if (tmList != null && tmList.Count > 0)
                            {
                                tms.AddRange(tmList);
                            }
                            tmList = await _examTmRepository.GetListByRandomAsync(allTm, hasTmGroup, tmIds, config.TxId, 0, 0, config.Nandu3TmCount, 0, 0);
                            if (tmList != null && tmList.Count > 0)
                            {
                                tms.AddRange(tmList);
                            }
                            tmList = await _examTmRepository.GetListByRandomAsync(allTm, hasTmGroup, tmIds, config.TxId, 0, 0, 0, config.Nandu4TmCount, 0);
                            if (tmList != null && tmList.Count > 0)
                            {
                                tms.AddRange(tmList);
                            }
                            tmList = await _examTmRepository.GetListByRandomAsync(allTm, hasTmGroup, tmIds, config.TxId, 0, 0, 0, 0, config.Nandu5TmCount);
                            if (tmList != null && tmList.Count > 0)
                            {
                                tms.AddRange(tmList);
                            }
                        }
                        if (tms.Count > 0)
                        {
                            await SetExamPaperRandomTm(tms, paper, randomId);
                        }
                    }
                }
            }
        }

        private async Task SetExamPaperRandomTm(List<ExamTm> tmList, ExamPaper paper, int randomId)
        {
            if (tmList != null && tmList.Count > 0)
            {
                var randomTmList = new List<ExamPaperRandomTm>();
                var tmCount = 0;
                decimal tmTotalScore = 0;

                var examConfig = await _examPaperRandomConfigRepository.GetListAsync(paper.Id);
                foreach (var tm in tmList)
                {
                    var tmScore = tm.Score;

                    if (paper.TmScoreType == ExamPaperTmScoreType.ScoreTypeTx)
                    {
                        tmScore = tm.Score;

                        var tx = await _examTxRepository.GetAsync(tm.TxId);
                        if (paper.TmRandomType == ExamPaperTmRandomType.RandomNone)
                        {
                            tmScore = tx.Score;
                        }
                        else
                        {
                            var configTx = examConfig.Single(config => config.TxId == tm.TxId);

                            if (configTx != null)
                            {
                                tmScore = configTx.TxScore;
                            }
                            else if (tx != null)
                            {
                                tmScore = tx.Score;
                            }
                        }
                    }

                    tmCount++;
                    tmTotalScore += tmScore;

                    var randomTm = new ExamPaperRandomTm
                    {
                        SourceTmId = tm.Id,
                        Score = tmScore,
                        Locked = tm.Locked,
                        Answer = tm.Answer,
                        AnswerCount = tm.AnswerCount,
                        CompanyId = tm.CompanyId,
                        CreatedDate = tm.CreatedDate,
                        CreatorId = tm.CreatorId,
                        DepartmentId = tm.DepartmentId,
                        ExamPaperId = paper.Id,
                        ExamPaperRandomId = randomId,
                        Guid = tm.Guid,
                        Jiexi = tm.Jiexi,
                        LastModifiedDate = tm.LastModifiedDate,
                        Nandu = tm.Nandu,
                        RightCount = tm.RightCount,
                        Title = tm.Title,
                        TreeId = tm.TreeId,
                        TxId = tm.TxId,
                        UseCount = tm.UseCount,
                        WrongCount = tm.WrongCount,
                        Zhishidian = tm.Zhishidian,
                    };

                    randomTm.Set("options", tm.Get("options"));
                    randomTm.Set("optionsValues", tm.Get("optionsValues"));
                    randomTmList.Add(randomTm);
                }
                if (paper.TmScoreType == ExamPaperTmScoreType.ScoreTypeRate)
                {
                    decimal rateTmTotalScore = 0;
                    foreach (var tm in randomTmList)
                    {
                        var tmScore = tm.Score;
                        try
                        {
                            tmScore = tm.Score / tmTotalScore * paper.TotalScore;
                            tmScore = Math.Round(tmScore, 2);
                        }
                        catch { }
                        tm.Score = tmScore;
                        rateTmTotalScore += tm.Score;
                    }

                    if (rateTmTotalScore < paper.TotalScore)
                    {
                        var plus = paper.TotalScore - rateTmTotalScore;
                        var lastTm = randomTmList[randomTmList.Count - 1];
                        lastTm.Score += plus;
                        randomTmList[randomTmList.Count - 1].Score = lastTm.Score;
                    }
                    if (rateTmTotalScore > paper.TotalScore)
                    {
                        var minus = rateTmTotalScore - paper.TotalScore;
                        var lastTm = randomTmList[randomTmList.Count - 1];
                        lastTm.Score -= minus;
                        randomTmList[randomTmList.Count - 1].Score = lastTm.Score;
                    }
                    tmTotalScore = paper.TotalScore;
                }
                else
                {
                    if (paper.PassScore > tmTotalScore)
                    {
                        paper.PassScore = Convert.ToInt32(tmTotalScore);
                    }
                }

                foreach (var tm in randomTmList)
                {
                    var randomTmId = await _examPaperRandomTmRepository.InsertAsync(tm);

                    var tx = await _examTxRepository.GetAsync(tm.TxId);
                    if (tx.ExamTxBase == ExamTxBase.Zuheti)
                    {
                        var smallList = await _examTmSmallRepository.GetListAsync(tm.SourceTmId);

                        if (smallList != null && smallList.Count > 0)
                        {
                            var smallIndex = 0;

                            var smallTotalScore = smallList.Sum(ss => ss.Score);
                            decimal smallTotalScoreNew = 0;

                            foreach (var small in smallList)
                            {
                                var newSmall = new ExamPaperRandomTmSmall()
                                {
                                    ParentId = randomTmId,
                                    SourceTmId = small.Id,
                                    ExamPaperId = paper.Id,
                                    ExamPaperRandomId = randomId,
                                    Score = small.Score,
                                    Locked = small.Locked,
                                    Answer = small.Answer,
                                    AnswerCount = small.AnswerCount,
                                    Jiexi = small.Jiexi,
                                    Title = small.Title,
                                    TxId = small.TxId,
                                };
                                newSmall.Set("options", small.Get("options"));
                                newSmall.Set("optionsValues", small.Get("optionsValues"));

                                smallIndex++;

                                if (smallTotalScore < tm.Score)
                                {
                                    if (smallIndex == smallList.Count)
                                    {
                                        newSmall.Score = newSmall.Score + (tm.Score - smallTotalScore);
                                    }
                                }
                                if (smallTotalScore > tm.Score)
                                {
                                    if (smallIndex == smallList.Count)
                                    {
                                        newSmall.Score = tm.Score - smallTotalScoreNew;
                                        if (newSmall.Score < 0)
                                        {
                                            newSmall.Score = 0;
                                        }
                                    }
                                    else
                                    {
                                        var newScore = small.Score / smallTotalScore * tm.Score;
                                        newSmall.Score = Math.Round(newScore, 2);
                                        smallTotalScoreNew += newSmall.Score;
                                    }
                                }


                                await _examPaperRandomTmSmallRepository.InsertAsync(newSmall);
                            }
                        }

                    }
                }

                paper.TmCount = tmCount;
                paper.TotalScore = (int)Math.Ceiling(tmTotalScore);
            }
        }
    }
}
