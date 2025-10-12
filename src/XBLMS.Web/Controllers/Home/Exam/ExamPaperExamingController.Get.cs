using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperExamingController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetList([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var paper = await _examPaperRepository.GetAsync(request.Id);

            if (paper == null) { return NotFound(); }

            var randomId = 0;
            var startId = 0;
            var isNewExam = true;

            if (request.LoadCounts > 0)
            {
                await _examManager.ClearRandomUser(request.Id, user.Id);
                await Task.Delay(1000);
            }

            var existCount = 0;
            var useTimeSecond = 0;
            var noSubmitStart = await _examPaperStartRepository.GetNoSubmitAsync(request.PlanId, request.CourseId, paper.Id, user.Id);
            if (noSubmitStart != null)
            {
                randomId = noSubmitStart.ExamPaperRandomId;
                startId = noSubmitStart.Id;
                isNewExam = false;
                useTimeSecond = noSubmitStart.ExamTimeSeconds;

                var timeSpan = DateTime.Now - noSubmitStart.BeginDateTime.Value;
                var useTotalSecond = timeSpan.TotalSeconds;
                if (useTotalSecond >= paper.TimingMinute * 60)
                {
                    useTotalSecond = paper.TimingMinute * 60;
                    useTimeSecond = -1;
                }
                else
                {
                    useTimeSecond = (int)useTotalSecond;
                }

                noSubmitStart.ExamTimeSeconds = (int)useTotalSecond;

                if (paper.ExistCount >= 0)
                {
                    noSubmitStart.ExistCount++;
                    existCount = noSubmitStart.ExistCount;
                }

                await _examPaperStartRepository.UpdateAsync(noSubmitStart);
            }
            else
            {
                if (paper.TmRandomType == ExamPaperTmRandomType.RandomExaming)
                {
                    await _examManager.SetExamPaperRantomByRandomNowAndExaming(paper, true);
                    randomId = await _examPaperRandomRepository.GetOneIdByPaperAsync(paper.Id);
                }
                else
                {
                    randomId = await _examPaperRandomRepository.GetOneIdByPaperAsync(paper.Id);
                }


                startId = await _examPaperStartRepository.InsertAsync(new ExamPaperStart
                {
                    PlanId = request.PlanId,
                    CourseId = request.CourseId,
                    UserId = user.Id,
                    ExamPaperId = paper.Id,
                    ExamPaperRandomId = randomId,
                    BeginDateTime = DateTime.Now,
                    KeyWords = paper.Title,
                    Moni = paper.Moni,
                    KeyWordsAdmin = await _organManager.GetUserKeyWords(user.Id),
                    CreatorId = user.Id,
                    CompanyId = user.CompanyId,
                    DepartmentId = user.DepartmentId,
                    CompanyParentPath = user.CompanyParentPath,
                    DepartmentParentPath = user.DepartmentParentPath
                });
            }


            if (randomId == 0) { return this.Error("试卷发布有问题，找不到任何题目，请联系管理员！"); }


            var configs = await _examPaperRandomConfigRepository.GetListAsync(paper.Id);
            if (configs == null || configs.Count == 0) { return this.Error("试卷发布有问题：找不到任何题目！"); }


            var paperTmTotal = 0;

            var tmIndex = 1;
            var haveSubjective = false;
            foreach (var config in configs)
            {
                var tx = await _examTxRepository.GetAsync(config.TxId);
                ExamTmType tmType = ExamTmType.Objective;
                if (tx.ExamTxBase == ExamTxBase.Tiankongti || tx.ExamTxBase == ExamTxBase.Jiandati)
                {
                    tmType = ExamTmType.Subjective;
                    haveSubjective = true;
                }

                var tms = await _examPaperRandomTmRepository.GetListAsync(randomId, config.TxId, paper.Id);
                if (tms != null && tms.Count > 0)
                {
                    paperTmTotal += tms.Count;
                    if (paper.IsExamingTmRandomView)
                    {
                        tms = tms.OrderBy(t => StringUtils.Guid()).ToList();
                    }
                    foreach (var item in tms)
                    {
                        if (isNewExam)
                        {
                            var examAnswer = new ExamPaperAnswer
                            {
                                UserId = user.Id,
                                ExamPaperId = paper.Id,
                                ExamStartId = startId,
                                RandomTmId = item.Id,
                                ExamTmType = tmType
                            };
                            examAnswer.Set("OptionsValues", new List<string>());
                            var answerId = await _examPaperAnswerRepository.InsertAsync(examAnswer);

                            if (tx.ExamTxBase == ExamTxBase.Zuheti)
                            {
                                var smallList = await _examPaperRandomTmSmallRepository.GetListAsync(item.Id);

                                if (smallList != null && smallList.Count > 0)
                                {
                                    var smallHasSubjective = false;
                                    foreach (var small in smallList)
                                    {
                                        var smalltx = await _examTxRepository.GetAsync(small.TxId);
                                        ExamTmType tmSmallType = ExamTmType.Objective;
                                        if (smalltx.ExamTxBase == ExamTxBase.Tiankongti || smalltx.ExamTxBase == ExamTxBase.Jiandati)
                                        {
                                            tmSmallType = ExamTmType.Subjective;
                                            smallHasSubjective = true;
                                            haveSubjective = true;
                                        }

                                        var examAnswerSmall = new ExamPaperAnswerSmall
                                        {
                                            AnswerId = answerId,
                                            UserId = user.Id,
                                            ExamPaperId = paper.Id,
                                            ExamStartId = startId,
                                            RandomTmId = small.Id,
                                            ExamTmType = tmSmallType
                                        };
                                        examAnswerSmall.Set("OptionsValues", new List<string>());
                                        await _examPaperAnswerSmallRepository.InsertAsync(examAnswerSmall);
                                    }
                                    if (smallHasSubjective)
                                    {
                                        examAnswer = await _examPaperAnswerRepository.GetAsync(answerId, paper.Id);
                                        examAnswer.ExamTmType = ExamTmType.Subjective;
                                        await _examPaperAnswerRepository.UpdateAsync(examAnswer);
                                    }
                                }
                            }

                        }

                        await _examManager.GetTmInfoByPaperUser(item, paper, startId);
                        item.Set("TmIndex", tmIndex);
                        tmIndex++;
                    }
                    config.Set("TmList", tms);
                }
            }

            var start = await _examPaperStartRepository.GetAsync(startId);
            start.HaveSubjective = haveSubjective;
            await _examPaperStartRepository.UpdateAsync(start);

            paper.Set("TmTotal", paperTmTotal);
            paper.Set("StartId", startId);

            paper.Set("UserDisplayName", user.DisplayName);
            paper.Set("UserAvatar", user.AvatarUrl);

            paper.Set("UseTimeSecond", useTimeSecond);

            paper.Set("ExistUserCount", existCount);

            var (aesKey, aesIV, aesSalt) = AesEncryptor.GetKey();

            return new GetResult
            {
                Watermark = await _authManager.GetWatermark(),
                Item = paper,
                TxList = AesEncryptor.Encrypt(TranslateUtils.JsonSerialize(configs), aesKey, aesIV),
                Salt = aesSalt
            };
        }
    }
}
