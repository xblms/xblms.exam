﻿using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ubiety.Dns.Core;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class ExamManager : IExamManager
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IPathManager _pathManager;
        private readonly ICreateManager _createManager;
        private readonly IUserRepository _userRepository;
        private readonly IExamTmRepository _examTmRepository;
        private readonly IExamTmSmallRepository _examTmSmallRepository;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamTmTreeRepository _examTmTreeRepository;
        private readonly IExamPaperTreeRepository _examPaperTreeRepository;
        private readonly IExamPaperRandomConfigRepository _examPaperRandomConfigRepository;
        private readonly IExamPaperRandomRepository _examPaperRandomRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamTmGroupRepository _examTmGroupRepository;
        private readonly IExamPaperRandomTmRepository _examPaperRandomTmRepository;
        private readonly IExamPaperRandomTmSmallRepository _examPaperRandomTmSmallRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamPaperAnswerRepository _examPaperAnswerRepository;
        private readonly IExamCerRepository _examCerRepository;
        private readonly IExamCerUserRepository _examCerUserRepository;
        private readonly IExamPracticeCollectRepository _examPracticeCollectRepository;
        private readonly IExamPracticeWrongRepository _examPracticeWrongRepository;
        private readonly IExamPracticeRepository _examPracticeRepository;
        private readonly IExamPaperAnswerSmallRepository _examPaperAnswerSmallRepository;
        private readonly IExamPracticeAnswerSmallRepository _examPracticeAnswerSmallRepository;

        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        private readonly IExamQuestionnaireAnswerRepository _examQuestionnaireAnswerRepository;
        private readonly IExamQuestionnaireTmRepository _examQuestionnaireTmRepository;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;

        private readonly IExamAssessmentRepository _examAssessmentRepository;
        private readonly IExamAssessmentUserRepository _examAssessmentUserRepository;
        private readonly IExamAssessmentTmRepository _examAssessmentTmRepository;
        private readonly IExamAssessmentAnswerRepository _examAssessmentAnswerRepository;
        private readonly IExamAssessmentConfigRepository _examAssessmentConfigRepository;
        private readonly IExamAssessmentConfigSetRepository _examAssessmentConfigSetRepository;

        private readonly IKnowlegesTreeRepository _knowlegesTreeRepository;
        private readonly IKnowlegesRepository _knowlegesRepository;

        private readonly IOrganManager _organManager;


        public ExamManager(ISettingsManager settingsManager,
            IOrganManager organManager,
            IPathManager pathManager,
            ICreateManager createManager,
            IExamTmRepository examTmRepository,
            IExamTxRepository examTxRepository,
            IExamTmTreeRepository examTmTreeRepository,
            IExamPaperTreeRepository examPaperTreeRepository,
            IExamPaperRandomConfigRepository examPaperRandomConfigRepository,
            IExamPaperRandomRepository examPaperRandomRepository,
            IExamPaperRepository examPaperRepository,
            IExamTmGroupRepository examTmGroupRepository,
            IExamPaperRandomTmRepository examPaperRandomTmRepository,
            IUserGroupRepository userGroupRepository,
            IUserRepository userRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IExamPaperAnswerRepository examPaperAnswerRepository,
            IExamCerRepository examCerRepository,
            IExamCerUserRepository examCerUserRepository,
            IExamPracticeCollectRepository examPracticeCollectRepository,
            IExamPracticeWrongRepository examPracticeWrongRepository,
            IExamPracticeRepository examPracticeRepository,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamQuestionnaireAnswerRepository examQuestionnaireAnswerRepository,
            IExamQuestionnaireTmRepository examQuestionnaireTmRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository,
            IExamAssessmentRepository examAssessmentRepository,
            IExamAssessmentUserRepository examAssessmentUserRepository,
            IExamAssessmentTmRepository examAssessmentTmRepository,
            IExamAssessmentAnswerRepository examAssessmentAnswerRepository,
            IExamAssessmentConfigRepository examAssessmentConfigRepository,
            IExamAssessmentConfigSetRepository examAssessmentConfigSetRepository,
            IKnowlegesTreeRepository knowlegesTreeRepository,
            IKnowlegesRepository knowlegesRepository,
            IExamTmSmallRepository examTmSmallRepository,
            IExamPaperRandomTmSmallRepository examPaperRandomTmSmallRepository,
            IExamPaperAnswerSmallRepository examPaperAnswerSmallRepository,
            IExamPracticeAnswerSmallRepository examPracticeAnswerSmallRepository)
        {
            _settingsManager = settingsManager;
            _organManager = organManager;
            _createManager = createManager;
            _pathManager = pathManager;
            _examTmRepository = examTmRepository;
            _examTxRepository = examTxRepository;
            _examTmTreeRepository = examTmTreeRepository;
            _examPaperTreeRepository = examPaperTreeRepository;
            _examPaperRandomConfigRepository = examPaperRandomConfigRepository;
            _examPaperRandomRepository = examPaperRandomRepository;
            _examPaperRepository = examPaperRepository;
            _examTmGroupRepository = examTmGroupRepository;
            _examPaperRandomTmRepository = examPaperRandomTmRepository;
            _userGroupRepository = userGroupRepository;
            _userRepository = userRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperStartRepository = examPaperStartRepository;
            _examPaperAnswerRepository = examPaperAnswerRepository;
            _examCerRepository = examCerRepository;
            _examCerUserRepository = examCerUserRepository;
            _examPracticeCollectRepository = examPracticeCollectRepository;
            _examPracticeWrongRepository = examPracticeWrongRepository;
            _examPracticeRepository = examPracticeRepository;
            _examQuestionnaireRepository = examQuestionnaireRepository;
            _examQuestionnaireAnswerRepository = examQuestionnaireAnswerRepository;
            _examQuestionnaireTmRepository = examQuestionnaireTmRepository;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
            _examAssessmentRepository = examAssessmentRepository;
            _examAssessmentUserRepository = examAssessmentUserRepository;
            _examAssessmentTmRepository = examAssessmentTmRepository;
            _examAssessmentAnswerRepository = examAssessmentAnswerRepository;
            _examAssessmentConfigRepository = examAssessmentConfigRepository;
            _examAssessmentConfigSetRepository = examAssessmentConfigSetRepository;
            _knowlegesTreeRepository = knowlegesTreeRepository;
            _knowlegesRepository = knowlegesRepository;
            _examTmSmallRepository = examTmSmallRepository;
            _examPaperRandomTmSmallRepository = examPaperRandomTmSmallRepository;
            _examPaperAnswerSmallRepository = examPaperAnswerSmallRepository;
            _examPracticeAnswerSmallRepository = examPracticeAnswerSmallRepository;
        }


        public async Task<ExamTm> GetTmInfo(int tmId)
        {
            var tm = await _examTmRepository.GetAsync(tmId);
            tm.Set("Options", tm.Get("options"));
            tm.Set("optionsValues", tm.Get("optionsValues"));
            return tm;
        }
        public async Task<ExamTmSmall> GetSmallTmInfo(int tmId)
        {
            var tm = await _examTmSmallRepository.GetAsync(tmId);
            tm.Set("Options", tm.Get("options"));
            tm.Set("optionsValues", tm.Get("optionsValues"));
            return tm;
        }
        public async Task GetBaseTmInfo(ExamTm tm)
        {
            tm.Set("Options", tm.Get("options"));
            tm.Set("OptionsValues", tm.Get("optionsValues"));

            tm.Set("TitleHtml", tm.Title);
            tm.Title = StringUtils.StripTags(tm.Title);

            tm.Set("NanduStar", $"{tm.Nandu}<i class='el-icon-star-on' style='color:#FF9900;margin-left:3px;font-size:14px;'></i>");

            var tx = await _examTxRepository.GetAsync(tm.TxId);
            tm.Set("TxName", tx.Name);
            tm.Set("TxTaxis", tx.Taxis);
            tm.Set("BaseTx", tx.ExamTxBase);

            var smallTmList = new List<ExamTmSmall>();
            if (tx.ExamTxBase == ExamTxBase.Zuheti)
            {
                var smallList = await _examTmSmallRepository.GetListAsync(tm.Id);
                if (smallList != null && smallList.Count > 0)
                {
                    for (var i = 0; i < smallList.Count; i++)
                    {
                        var smallTm = smallList[i];
                        var smallTx = await _examTxRepository.GetAsync(smallTm.TxId);

                        smallTm.Set("Options", smallTm.Get("options"));
                        smallTm.Set("OptionsValues", smallTm.Get("optionsValues"));

                        smallTm.Set("TitleHtml", smallTm.Title);

                        var tmTitleList = GetTiankongtiTitleList(smallTx, smallTm.Title);

                        smallTm.Title = StringUtils.StripTags(smallTm.Title);

                        var smalltx = await _examTxRepository.GetAsync(smallTm.TxId);
                        smallTm.Set("TxName", smalltx.Name);
                        smallTm.Set("TxTaxis", smalltx.Taxis);
                        smallTm.Set("BaseTx", smalltx.ExamTxBase);


                        smallTm.Set("TitleList", tmTitleList);
                        smallTm.Set("OptionsValues", new List<string>());
                        smallTm.Set("OptionsRandom", GetOptionRandm(null, ListUtils.ToList(smallTm.Get("options"))));

                        smallTm.Set("MyAnswer", "");

                        smallTmList.Add(smallTm);
                    }
                }
            }
            tm.Set("SmallList", smallTmList);
        }

        public async Task GetTmDeleteInfo(ExamTm tm)
        {
            tm.Set("Options", tm.Get("options"));
            tm.Set("OptionsValues", tm.Get("optionsValues"));

            tm.Set("TitleHtml", tm.Title);
            tm.Title = StringUtils.StripTags(tm.Title);

            tm.Set("NanduStar", $"{tm.Nandu}<i class='el-icon-star-on' style='color:#FF9900;margin-left:3px;font-size:14px;'></i>");

            var tx = await _examTxRepository.GetAsync(tm.TxId);
            if (tx != null)
            {
                tm.Set("TxName", tx.Name);
                tm.Set("TxTaxis", tx.Taxis);
                tm.Set("BaseTx", tx.ExamTxBase);
            }
            else
            {
                tm.Set("TxName", "题型不存在");
                tm.Set("TxTaxis", "题型不存在");
                tm.Set("BaseTx", "题型不存在");
            }

            var treeName = await _examTmTreeRepository.GetPathNamesAsync(tm.TreeId);
            tm.Set("TreeName", treeName);

        }
        public async Task GetTmInfo(ExamTm tm)
        {
            await GetBaseTmInfo(tm);

            var treeName = await _examTmTreeRepository.GetPathNamesAsync(tm.TreeId);
            tm.Set("TreeName", treeName);

        }

        private List<KeyValuePair<int, string>> GetTiankongtiTitleList(ExamTx tx, string title)
        {
            var tmTitleList = new List<KeyValuePair<int, string>>();
            if (tx.ExamTxBase == ExamTxBase.Tiankongti && title.Contains("___"))
            {
                string newTitle = StringUtils.Replace(title, "___", "|___|");
                var tmTitleSplitList = ListUtils.GetStringList(newTitle, '|');
                var inputIndex = 0;
                for (int i = 0; i < tmTitleSplitList.Count; i++)
                {
                    if (tmTitleSplitList[i].Contains("___"))
                    {
                        tmTitleList.Add(new KeyValuePair<int, string>(inputIndex, tmTitleSplitList[i]));
                        inputIndex++;
                    }
                    else
                    {
                        tmTitleList.Add(new KeyValuePair<int, string>(i, tmTitleSplitList[i]));
                    }
                }
            }
            return tmTitleList;
        }
        public async Task GetSmallListByPaper(ExamPaperRandomTm tm, int startId, ExamPaper paper)
        {
            var tx = await _examTxRepository.GetAsync(tm.TxId);
            var smallTmList = new List<ExamPaperRandomTmSmall>();
            if (tx.ExamTxBase == ExamTxBase.Zuheti)
            {
                var smallList = await _examPaperRandomTmSmallRepository.GetListAsync(tm.Id);
                if (smallList != null && smallList.Count > 0)
                {
                    for (var i = 0; i < smallList.Count; i++)
                    {
                        var smallTm = smallList[i];
                        var smallTx = await _examTxRepository.GetAsync(smallTm.TxId);

                        var options = smallTm.Get("options");
                        smallTm.Set("Options", options);
                        smallTm.Set("OptionsValues", smallTm.Get("optionsValues"));

                        smallTm.Set("TitleHtml", smallTm.Title);

                        var tmTitleList = GetTiankongtiTitleList(smallTx, smallTm.Title);

                        smallTm.Title = StringUtils.StripTags(smallTm.Title);

                        var smalltx = await _examTxRepository.GetAsync(smallTm.TxId);
                        smallTm.Set("TxName", smalltx.Name);
                        smallTm.Set("TxTaxis", smalltx.Taxis);
                        smallTm.Set("BaseTx", smalltx.ExamTxBase);


                        smallTm.Set("TitleList", tmTitleList);

                        if (startId > 0 && paper != null)
                        {
                            var answerStatus = true;
                            var answer = await _examPaperAnswerSmallRepository.GetAsync(smallTm.Id, startId, paper.Id);
                            if (string.IsNullOrWhiteSpace(answer.Answer))
                            {
                                answerStatus = false;
                            }


                            smallTm.Set("AnswerInfo", answer);
                            smallTm.Set("AnswerStatus", answerStatus);

                            smallTm.Set("IsRight", StringUtils.Equals(smallTm.Answer, answer.Answer) || answer.Score > 0);
                            if (!paper.SecrecyPaperAnswer)
                            {
                                smallTm.Answer = "";
                            }

                            var answerStatue = answer.Get("MarkState");

                            var markState = false;
                            if (answerStatue != null)
                            {
                                markState = TranslateUtils.ToBool(answer.Get("MarkState").ToString());
                            }

                            smallTm.Set("MarkState", markState);
                        }


                        smallTm.Set("OptionsRandom", GetOptionRandm(paper, ListUtils.ToList(options), true));
                        smallTm.Set("TmIndex", i + 1);

                        smallTmList.Add(smallTm);
                    }
                }
            }
            tm.Set("SmallLists", smallTmList);
        }
        public async Task GetTmInfoByPaper(ExamTm tm)
        {
            await GetBaseTmInfo(tm);
            tm.Title = tm.Get("TitleHtml").ToString();
            var tx = await _examTxRepository.GetAsync(tm.TxId);

            var tmTitleList = GetTiankongtiTitleList(tx, tm.Title);

            tm.Set("TitleList", tmTitleList);
        }

        public List<KeyValuePair<string, string>> GetOptionRandm(ExamPaper paper, List<string> options, bool paperView = false)
        {
            var optionsRandom = new List<KeyValuePair<string, string>>();
            var abcList = StringUtils.GetABC();
            for (var i = 0; i < options.Count; i++)
            {
                optionsRandom.Add(new KeyValuePair<string, string>(abcList[i], options[i]));
            }
            if (paper != null && paper.IsExamingTmOptionRandomView && !paperView)
            {
                optionsRandom = ListUtils.GetRandomList(optionsRandom);
            }
            return optionsRandom;
        }

        public async Task GetTmInfoByPaperUser(ExamPaperRandomTm tm, ExamPaper paper, int startId, bool paperView = false)
        {
            await GetTmInfoByPaper(tm);

            tm.Set("OptionsRandom", GetOptionRandm(paper, ListUtils.ToList(tm.Get("options")), paperView));

            var answerStatus = true;
            var answer = await _examPaperAnswerRepository.GetAsync(tm.Id, startId, paper.Id);
            if (string.IsNullOrWhiteSpace(answer.Answer))
            {
                answerStatus = false;
            }


            tm.Set("AnswerInfo", answer);
            tm.Set("AnswerStatus", answerStatus);

            if (paperView)
            {
                await GetTmIsRightByView(tm, answer);
                if (!paper.SecrecyPaperAnswer)
                {
                    tm.Answer = string.Empty;
                }
            }
            else
            {
                tm.Answer = string.Empty;
            }

            await GetSmallListByPaper(tm, startId, paper);

        }
        public async Task GetTmInfoByPaperViewAdmin(ExamPaperRandomTm tm, ExamPaper paper, int startId)
        {
            await GetTmInfoByPaper(tm);

            tm.Set("OptionsRandom", GetOptionRandm(paper, ListUtils.ToList(tm.Get("options"))));

            var answerStatus = true;
            var answer = await _examPaperAnswerRepository.GetAsync(tm.Id, startId, paper.Id);
            if (string.IsNullOrWhiteSpace(answer.Answer))
            {
                answerStatus = false;
            }

            tm.Set("AnswerInfo", answer);
            tm.Set("AnswerStatus", answerStatus);

            await GetTmIsRightByView(tm, answer);
            await GetSmallListByPaper(tm, startId, paper);
        }
        public async Task GetTmInfoByPaperMark(ExamPaperRandomTm tm, ExamPaper paper, int startId)
        {
            await GetTmInfoByPaper(tm);

            tm.Set("OptionsRandom", GetOptionRandm(paper, ListUtils.ToList(tm.Get("options"))));

            var answer = await _examPaperAnswerRepository.GetAsync(tm.Id, startId, paper.Id);

            var answerStatue = answer.Get("MarkState");

            var markState = false;
            if (answerStatue != null)
            {
                markState = TranslateUtils.ToBool(answer.Get("MarkState").ToString());
            }

            tm.Set("AnswerInfo", answer);
            tm.Set("MarkState", markState);

            await GetTmIsRightByView(tm, answer);
            await GetSmallListByPaper(tm, startId, paper);
        }
        public async Task GetTmIsRightByView(ExamPaperRandomTm tm, ExamPaperAnswer answer)
        {
            var tx = await _examTxRepository.GetAsync(tm.TxId);
            var isRight = StringUtils.Equals(tm.Answer, answer.Answer) || answer.Score > 0;
            if (tx.ExamTxBase == ExamTxBase.Zuheti)
            {
                isRight = answer.Score == tm.Score;
            }
            tm.Set("IsRight", isRight);
        }
        public async Task GetSmallTmListByPracticeView(ExamTm tm, int practiceId)
        {
            var tx = await _examTxRepository.GetAsync(tm.TxId);
            var smallTmList = new List<ExamTmSmall>();
            if (tx.ExamTxBase == ExamTxBase.Zuheti)
            {
                var smallList = await _examTmSmallRepository.GetListAsync(tm.Id);
                if (smallList != null && smallList.Count > 0)
                {
                    for (var i = 0; i < smallList.Count; i++)
                    {
                        var smallTm = smallList[i];
                        var smallTx = await _examTxRepository.GetAsync(smallTm.TxId);

                        var options = smallTm.Get("options");
                        smallTm.Set("Options", options);
                        smallTm.Set("OptionsValues", smallTm.Get("optionsValues"));

                        smallTm.Set("TitleHtml", smallTm.Title);

                        var tmTitleList = GetTiankongtiTitleList(smallTx, smallTm.Title);

                        smallTm.Title = StringUtils.StripTags(smallTm.Title);

                        var smalltx = await _examTxRepository.GetAsync(smallTm.TxId);
                        smallTm.Set("TxName", smalltx.Name);
                        smallTm.Set("TxTaxis", smalltx.Taxis);
                        smallTm.Set("BaseTx", smalltx.ExamTxBase);


                        smallTm.Set("TitleList", tmTitleList);

                        var answer = await _examPracticeAnswerSmallRepository.GetAsync(smallTm.Id, practiceId);

                        smallTm.Set("AnswerInfo", answer);
                        smallTm.Set("IsRight", StringUtils.Equals(smallTm.Answer, answer.Answer));


                        smallTm.Set("OptionsRandom", GetOptionRandm(null, ListUtils.ToList(options), true));
                        smallTm.Set("TmIndex", i + 1);

                        smallTmList.Add(smallTm);
                    }
                }
            }
            tm.Set("SmallLists", smallTmList);
        }
        public async Task GetTmInfoByPracticing(ExamTm tm)
        {
            await GetTmInfoByPaper(tm);

            tm.Set("OptionsRandom", GetOptionRandm(null, ListUtils.ToList(tm.Get("options"))));
            tm.Answer = "";
            tm.Set("OptionsValues", new List<string>());

        }
        public async Task GetTmInfoByPracticeView(ExamTm tm, int practiceId)
        {
            await GetTmInfoByPaper(tm);
            await GetSmallTmListByPracticeView(tm, practiceId);
            tm.Set("OptionsRandom", GetOptionRandm(null, ListUtils.ToList(tm.Get("options"))));
        }

        public async Task GetPaperInfo(ExamPaper paper, User user, bool cjList = false)
        {
            var myExamTimes = await _examPaperStartRepository.CountAsync(paper.Id, user.Id);
            var startId = await _examPaperStartRepository.GetNoSubmitIdAsync(paper.Id, user.Id);
            var cerName = "";
            if (paper.CerId > 0)
            {
                var cer = await _examCerRepository.GetAsync(paper.CerId);
                if (cer != null)
                {
                    cerName = cer.Name;
                }
            }

            var examUser = await _examPaperUserRepository.GetAsync(paper.Id, user.Id);


            paper.Set("StartId", startId);
            paper.Set("CerName", cerName);
            paper.Set("ExamStartDateTimeStr", DateUtils.Format(examUser.ExamBeginDateTime, DateUtils.FormatStringDateTimeCN));
            paper.Set("ExamEndDateTimeStr", DateUtils.Format(examUser.ExamEndDateTime, DateUtils.FormatStringDateTimeCN));

            paper.Set("MyExamTimes", myExamTimes > examUser.ExamTimes ? examUser.ExamTimes : myExamTimes);
            paper.Set("UserExamTimes", examUser.ExamTimes);

            double longTime = 0;
            if (examUser.ExamBeginDateTime.Value > DateTime.Now)
            {
                var timeSpan = DateUtils.DateTimeToUnixTimestamp(examUser.ExamBeginDateTime.Value);
                longTime = timeSpan;
            }
            paper.Set("ExamStartDateTimeLong", longTime);

            var taskStartIds = _createManager.GetTaskStartIds();
            if (taskStartIds.Contains(startId))
            {
                paper.Set("ExamSubmiting", true);
            }
            else
            {
                paper.Set("ExamSubmiting", false);
            }
            if (cjList)
            {
                var cjlist = await _examPaperStartRepository.GetListAsync(paper.Id, user.Id);
                if (cjlist != null && cjlist.Count > 0)
                {
                    foreach (var cj in cjlist)
                    {
                        if (!paper.SecrecyScore)
                        {
                            cj.Score = 0;
                        }
                        cj.Set("UseTime", DateUtils.SecondToHms(cj.ExamTimeSeconds));

                    }
                }
                paper.Set("CjList", cjlist);
            }
        }
        public async Task GetPaperInfo(ExamPaper paper, User user, ExamPaperStart start)
        {
            var myExamTimes = await _examPaperStartRepository.CountAsync(paper.Id, user.Id);
            var startId = await _examPaperStartRepository.GetNoSubmitIdAsync(paper.Id, user.Id);
            var cerName = "";
            if (paper.CerId > 0)
            {
                var cer = await _examCerRepository.GetAsync(paper.CerId);
                if (cer != null)
                {
                    cerName = cer.Name;
                }
            }

            var examUser = await _examPaperUserRepository.GetAsync(paper.Id, user.Id);


            paper.Set("StartId", startId);
            paper.Set("CerName", cerName);
            paper.Set("ExamStartDateTimeStr", DateUtils.Format(start.BeginDateTime, DateUtils.FormatStringDateTimeCN));
            paper.Set("ExamEndDateTimeStr", DateUtils.Format(start.EndDateTime, DateUtils.FormatStringDateTimeCN));

            paper.Set("MyExamTimes", myExamTimes > examUser.ExamTimes ? examUser.ExamTimes : myExamTimes);
            paper.Set("UserExamTimes", examUser.ExamTimes);
        }
        public async Task GetQuestionnaireInfo(ExamQuestionnaire paper, User user)
        {
            var paperUser = await _examQuestionnaireUserRepository.GetAsync(paper.Id, user.Id);
            paper.Set("ExamStartDateTimeStr", DateUtils.Format(paper.ExamBeginDateTime, DateUtils.FormatStringDateTimeCN));
            paper.Set("ExamEndDateTimeStr", DateUtils.Format(paper.ExamEndDateTime, DateUtils.FormatStringDateTimeCN));
            paper.Set("SubmitType", paperUser.SubmitType);
            paper.Set("State", DateTime.Now >= paper.ExamBeginDateTime.Value && DateTime.Now <= paper.ExamEndDateTime.Value);

        }
        public void GetExamAssessmentInfo(ExamAssessment ass, ExamAssessmentUser assUser, User user)
        {
            ass.Set("ExamStartDateTimeStr", DateUtils.Format(ass.ExamBeginDateTime, DateUtils.FormatStringDateTimeCN));
            ass.Set("ExamEndDateTimeStr", DateUtils.Format(ass.ExamEndDateTime, DateUtils.FormatStringDateTimeCN));
            ass.Set("SubmitType", assUser.SubmitType);
            ass.Set("State", DateTime.Now >= ass.ExamBeginDateTime.Value && DateTime.Now <= ass.ExamEndDateTime.Value);
            ass.Set("ConfigId", assUser.ConfigId);
            ass.Set("ConfigName", assUser.ConfigName);

        }
        public async Task<(bool Success, string msg)> CheckExam(int paperId, int userId)
        {
            var paper = await _examPaperRepository.GetAsync(paperId);
            var paperUser = await _examPaperUserRepository.GetAsync(paperId, userId);
            if (paper != null || paperUser != null)
            {
                var myTimes = await _examPaperStartRepository.CountAsync(paperId, userId);
                var times = paperUser.ExamTimes;
                if (times - myTimes <= 0)
                {
                    return (false, "剩余考试次数不足");
                }

                if (paperUser.ExamBeginDateTime.Value > DateTime.Now)
                {
                    return (false, "考试未开始，请耐心等待");
                }

                if (paperUser.ExamEndDateTime.Value < DateTime.Now)
                {
                    return (false, "考试已过期");
                }
            }
            else
            {
                return (false, "未找到试卷");
            }

            return (true, "");
        }
    }
}
