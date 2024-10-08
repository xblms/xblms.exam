using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamTmTreeRepository _examTmTreeRepository;
        private readonly IExamPaperTreeRepository _examPaperTreeRepository;
        private readonly IExamPaperRandomConfigRepository _examPaperRandomConfigRepository;
        private readonly IExamPaperRandomRepository _examPaperRandomRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamTmGroupRepository _examTmGroupRepository;
        private readonly IExamPaperRandomTmRepository _examPaperRandomTmRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamPaperAnswerRepository _examPaperAnswerRepository;
        private readonly IExamCerRepository _examCerRepository;
        private readonly IExamCerUserRepository _examCerUserRepository;
        private readonly IExamPracticeCollectRepository _examPracticeCollectRepository;
        private readonly IExamPracticeWrongRepository _examPracticeWrongRepository;
        private readonly IExamPracticeRepository _examPracticeRepository;

        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        private readonly IExamQuestionnaireAnswerRepository _examQuestionnaireAnswerRepository;
        private readonly IExamQuestionnaireTmRepository _examQuestionnaireTmRepository;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;

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
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository)
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
        }


        public async Task<ExamTm> GetTmInfo(int tmId)
        {
            var tm = await _examTmRepository.GetAsync(tmId);
            tm.Set("Options", tm.Get("options"));
            tm.Set("optionsValues", tm.Get("optionsValues"));
            return tm;
        }
        public async Task GetBaseTmInfo(ExamTm tm)
        {
            tm.Set("Options", tm.Get("options"));
            tm.Set("OptionsValues", tm.Get("optionsValues"));
            tm.Title = StringUtils.StripTags(tm.Title);

            tm.Set("TitleHtml", tm.Title);
            tm.Set("NanduStar", $"{tm.Nandu}<i class='el-icon-star-on' style='color:#FF9900;margin-left:3px;font-size:14px;'></i>");

            var tx = await _examTxRepository.GetAsync(tm.TxId);
            tm.Set("TxName", tx.Name);
            tm.Set("TxTaxis", tx.Taxis);
            tm.Set("BaseTx", tx.ExamTxBase);


        }
        public async Task GetTmInfo(ExamTm tm)
        {
            await GetBaseTmInfo(tm);

            var treeName = await _examTmTreeRepository.GetPathNamesAsync(tm.TreeId);
            tm.Set("TreeName", treeName);

        }

        public async Task GetTmInfoByPaper(ExamTm tm)
        {
            await GetBaseTmInfo(tm);

            var tx = await _examTxRepository.GetAsync(tm.TxId);

            var tmTitleList = new List<KeyValuePair<int, string>>();
            if (tx.ExamTxBase == ExamTxBase.Tiankongti && tm.Title.Contains("___"))
            {
                string newTitle = StringUtils.Replace(tm.Title, "___", "|___|");
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

            tm.Set("TitleList", tmTitleList);
        }



        public async Task GetTmInfoByPaperUser(ExamPaperRandomTm tm, ExamPaper paper, int startId, bool paperView = false)
        {
            await GetTmInfoByPaper(tm);

            var optionsRandom = new List<KeyValuePair<string, string>>();
            var options = ListUtils.ToList(tm.Get("options"));
            var abcList = StringUtils.GetABC();
            for (var i = 0; i < options.Count; i++)
            {
                optionsRandom.Add(new KeyValuePair<string, string>(abcList[i], options[i]));
            }
            if (paper.IsExamingTmOptionRandomView)
            {
                optionsRandom = ListUtils.GetRandomList(optionsRandom);
            }
            tm.Set("OptionsRandom", optionsRandom);

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
                tm.Set("IsRight", StringUtils.Equals(tm.Answer, answer.Answer) || answer.Score > 0);
                if (!paper.SecrecyPaperAnswer)
                {
                    tm.Answer = "";
                }
            }
            else
            {
                tm.Answer = "";
            }


        }
        public async Task GetTmInfoByPaperViewAdmin(ExamPaperRandomTm tm, ExamPaper paper, int startId)
        {
            await GetTmInfoByPaper(tm);

            var optionsRandom = new List<KeyValuePair<string, string>>();
            var options = ListUtils.ToList(tm.Get("options"));
            var abcList = StringUtils.GetABC();
            for (var i = 0; i < options.Count; i++)
            {
                optionsRandom.Add(new KeyValuePair<string, string>(abcList[i], options[i]));
            }

            tm.Set("OptionsRandom", optionsRandom);

            var answerStatus = true;
            var answer = await _examPaperAnswerRepository.GetAsync(tm.Id, startId, paper.Id);
            if (string.IsNullOrWhiteSpace(answer.Answer))
            {
                answerStatus = false;
            }

            tm.Set("AnswerInfo", answer);
            tm.Set("AnswerStatus", answerStatus);
            tm.Set("IsRight", StringUtils.Equals(tm.Answer, answer.Answer) || answer.Score > 0);

        }
        public async Task GetTmInfoByPaperMark(ExamPaperRandomTm tm, ExamPaper paper, int startId)
        {
            await GetTmInfoByPaper(tm);

            var optionsRandom = new List<KeyValuePair<string, string>>();
            var options = ListUtils.ToList(tm.Get("options"));
            var abcList = StringUtils.GetABC();
            for (var i = 0; i < options.Count; i++)
            {
                optionsRandom.Add(new KeyValuePair<string, string>(abcList[i], options[i]));
            }

            tm.Set("OptionsRandom", optionsRandom);

            var answer = await _examPaperAnswerRepository.GetAsync(tm.Id, startId, paper.Id);

            var answerStatue = answer.Get("MarkState");

            var markState = false;
            if (answerStatue != null)
            {
                markState = TranslateUtils.ToBool(answer.Get("MarkState").ToString());
            }

            tm.Set("AnswerInfo", answer);
            tm.Set("MarkState", markState);
            tm.Set("IsRight", StringUtils.Equals(tm.Answer, answer.Answer) || answer.Score > 0);

        }

        public async Task GetTmInfoByPracticing(ExamTm tm)
        {
            await GetTmInfoByPaper(tm);

            var optionsRandom = new List<KeyValuePair<string, string>>();
            var options = ListUtils.ToList(tm.Get("options"));
            var abcList = StringUtils.GetABC();
            for (var i = 0; i < options.Count; i++)
            {
                optionsRandom.Add(new KeyValuePair<string, string>(abcList[i], options[i]));
            }

            tm.Set("OptionsRandom", optionsRandom);
            tm.Answer = "";
            tm.Set("OptionsValues", new List<string>());
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
        public async Task GetQuestionnaireInfo(ExamQuestionnaire paper, User user)
        {
            var paperUser = await _examQuestionnaireUserRepository.GetAsync(paper.Id, user.Id);
            paper.Set("ExamStartDateTimeStr", DateUtils.Format(paper.ExamBeginDateTime, DateUtils.FormatStringDateTimeCN));
            paper.Set("ExamEndDateTimeStr", DateUtils.Format(paper.ExamEndDateTime, DateUtils.FormatStringDateTimeCN));
            paper.Set("SubmitType", paperUser.SubmitType);
            paper.Set("State", DateTime.Now >= paper.ExamBeginDateTime && DateTime.Now <= paper.ExamEndDateTime);

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
