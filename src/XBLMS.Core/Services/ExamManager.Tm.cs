using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task<ExamTm> GetTmInfo(int tmId)
        {
            var tm = await _databaseManager.ExamTmRepository.GetAsync(tmId);
            SetOptionInfo(tm);
            return tm;
        }

        public async Task GetTmInfo(ExamTm tm)
        {
            await GetBaseTmInfo(tm);

            var treeName = await _databaseManager.ExamTmTreeRepository.GetPathNamesAsync(tm.TreeId);
            tm.Set("TreeName", treeName);
        }

        public async Task GetTmInfoByPaper(ExamTm tm)
        {
            await GetBaseTmInfo(tm);
            tm.Title = tm.Get("TitleHtml").ToString();
            var tx = await _databaseManager.ExamTxRepository.GetAsync(tm.TxId);

            var tmTitleList = GetTiankongtiTitleList(tx, tm.Title);
            tm.Set("TitleList", tmTitleList);
        }
        public async Task GetSmallListByPaper(ExamPaperRandomTm tm, int startId, ExamPaper paper)
        {
            var tx = await _databaseManager.ExamTxRepository.GetAsync(tm.TxId);
            var smallTmList = new List<ExamPaperRandomTmSmall>();
            if (tx.ExamTxBase == ExamTxBase.Zuheti)
            {
                var smallList = await _databaseManager.ExamPaperRandomTmSmallRepository.GetListAsync(tm.Id);
                if (smallList != null && smallList.Count > 0)
                {
                    for (var i = 0; i < smallList.Count; i++)
                    {
                        var smallTm = smallList[i];
                        var smallTx = await _databaseManager.ExamTxRepository.GetAsync(smallTm.TxId);

                        var options = smallTm.Get("options");
                        smallTm.Set("Options", options);
                        smallTm.Set("OptionsValues", smallTm.Get("optionsValues"));

                        smallTm.Set("TitleHtml", smallTm.Title);

                        var tmTitleList = GetTiankongtiTitleList(smallTx, smallTm.Title);

                        smallTm.Title = StringUtils.StripTags(smallTm.Title);

                        var smalltx = await _databaseManager.ExamTxRepository.GetAsync(smallTm.TxId);
                        smallTm.Set("TxName", smalltx.Name);
                        smallTm.Set("TxTaxis", smalltx.Taxis);
                        smallTm.Set("BaseTx", smalltx.ExamTxBase);
                        smallTm.Set("TitleList", tmTitleList);

                        if (startId > 0 && paper != null)
                        {
                            var answerStatus = true;
                            var answer = await _databaseManager.ExamPaperAnswerSmallRepository.GetAsync(smallTm.Id, startId, paper.Id);
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


                        GetTmOptionsRandom(smallTm);
                        smallTm.Set("TmIndex", i + 1);

                        smallTmList.Add(smallTm);
                    }
                }
            }
            tm.Set("SmallLists", smallTmList);
        }
        public async Task GetTmInfoByPaperUser(ExamPaperRandomTm tm, ExamPaper paper, int startId, bool paperView = false)
        {
            await GetTmInfoByPaper(tm);

            await GetTmAnswer(tm, startId, paper.Id);

            if (paperView)
            {
                if (!paper.SecrecyPaperAnswer)
                {
                    tm.Answer = "";
                }
            }
            else
            {
                tm.Answer = "";
            }
            GetTmOptionsRandom(tm, paper.IsExamingTmOptionRandomView && !paperView);

            await GetSmallListByPaper(tm, startId, paper);
        }
        public async Task GetTmInfoByPaperAdmin(ExamPaperRandomTm tm, ExamPaper paper, int startId)
        {
            await GetTmInfoByPaper(tm);

            await GetTmAnswer(tm, startId, paper.Id);

            GetTmOptionsRandom(tm);

            await GetSmallListByPaper(tm, startId, paper);
        }


        private async Task GetTmAnswer(ExamTm tm, int startId, int paperId)
        {
            if (startId > 0)
            {
                var answer = await _databaseManager.ExamPaperAnswerRepository.GetAsync(tm.Id, startId, paperId);

                var answerStatus = true;
                if (string.IsNullOrWhiteSpace(answer.Answer))
                {
                    answerStatus = false;
                }
                tm.Set("AnswerStatus", answerStatus);

                var markState = false;
                if (answer.Get("MarkState") != null)
                {
                    markState = TranslateUtils.ToBool(answer.Get("MarkState").ToString());
                }

                tm.Set("MarkState", markState);

                tm.Set("IsRight", StringUtils.Equals(tm.Answer, answer.Answer) || answer.Score > 0);

                tm.Set("AnswerInfo", answer);
            }
       
        }

        private void GetTmOptionsRandom(ExamTm tm, bool isRandom = false)
        {
            var optionsRandom = new List<KeyValuePair<string, string>>();
            var options = ListUtils.ToList(tm.Get("options"));
            var abcList = StringUtils.GetABC();
            for (var i = 0; i < options.Count; i++)
            {
                optionsRandom.Add(new KeyValuePair<string, string>(abcList[i], options[i]));
            }
            if (isRandom)
            {
                optionsRandom = ListUtils.GetRandomList(optionsRandom);
            }
            tm.Set("OptionsRandom", optionsRandom);
        }

        private void SetOptionInfo(ExamTm tm)
        {
            tm.Set("Options", tm.Get("options"));
            tm.Set("OptionsValues", tm.Get("optionsValues"));
        }
        private async Task GetBaseTmInfo(ExamTm tm)
        {
            SetOptionInfo(tm);

            tm.Set("TitleHtml", tm.Title);
            tm.Title = StringUtils.StripTags(tm.Title);

            tm.Set("NanduStar", $"{tm.Nandu}<i class='el-icon-star-on' style='color:#FF9900;margin-left:3px;font-size:14px;'></i>");

            var tx = await _databaseManager.ExamTxRepository.GetAsync(tm.TxId);
            tm.Set("TxName", tx.Name);
            tm.Set("TxTaxis", tx.Taxis);
            tm.Set("BaseTx", tx.ExamTxBase);


            var smallTmList = new List<ExamTmSmall>();
            if (tx.ExamTxBase == ExamTxBase.Zuheti)
            {
                var smallList = await _databaseManager.ExamTmSmallRepository.GetListAsync(tm.Id);
                if (smallList != null && smallList.Count > 0)
                {
                    for (var i = 0; i < smallList.Count; i++)
                    {
                        var smallTm = smallList[i];
                        await GetSmallTm(smallTm);
                        smallTmList.Add(smallTm);
                    }
                }
            }
            tm.Set("SmallList", smallTmList);
        }
        private async Task GetSmallTm(ExamTmSmall smallTm)
        {
            var smallTx = await _databaseManager.ExamTxRepository.GetAsync(smallTm.TxId);

            SetOptionInfo(smallTm);

            smallTm.Set("TitleHtml", smallTm.Title);

            var tmTitleList = GetTiankongtiTitleList(smallTx, smallTm.Title);

            smallTm.Title = StringUtils.StripTags(smallTm.Title);

            var smalltx = await _databaseManager.ExamTxRepository.GetAsync(smallTm.TxId);
            smallTm.Set("TxName", smalltx.Name);
            smallTm.Set("TxTaxis", smalltx.Taxis);
            smallTm.Set("BaseTx", smalltx.ExamTxBase);

            smallTm.Set("TitleList", tmTitleList);
            smallTm.Set("OptionsValues", new List<string>());

            smallTm.Set("MyAnswer", "");

            GetTmOptionsRandom(smallTm);
        }
        public async Task<ExamTmSmall> GetSmallTmInfo(int tmId)
        {
            var tm = await _databaseManager.ExamTmSmallRepository.GetAsync(tmId);
            SetOptionInfo(tm);
            return tm;
        }

        public async Task GetTmDeleteInfo(ExamTm tm)
        {
            SetOptionInfo(tm);

            tm.Set("TitleHtml", tm.Title);
            tm.Title = StringUtils.StripTags(tm.Title);

            tm.Set("NanduStar", $"{tm.Nandu}<i class='el-icon-star-on' style='color:#FF9900;margin-left:3px;font-size:14px;'></i>");

            var tx = await _databaseManager.ExamTxRepository.GetAsync(tm.TxId);
            if (tx != null)
            {
                tm.Set("TxName", tx.Name);
                tm.Set("TxTaxis", tx.Taxis);
                tm.Set("BaseTx", tx.ExamTxBase);
            }
            else
            {
                var txMiss = "题型不存在";
                tm.Set("TxName", txMiss);
                tm.Set("TxTaxis", txMiss);
                tm.Set("BaseTx", txMiss);
            }

            var treeName = await _databaseManager.ExamTmTreeRepository.GetPathNamesAsync(tm.TreeId);
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
    }
}
