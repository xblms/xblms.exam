using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireAnalysisController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }

            var paper = await _questionnaireRepository.GetAsync(request.Id);
            var tmList = await _questionnaireTmRepository.GetListAsync(paper.Id);

            var tmTotal = 0;
            if (tmList != null && tmList.Count > 0)
            {
                var tmIndex = 0;
                foreach (var tm in tmList)
                {
                    if (tm.ParentId == 0)
                    {
                        tmIndex++; tmTotal++;
                    }

                    if (tm.Tx == ExamQuestionnaireTxType.DanxuantiErwei || tm.Tx == ExamQuestionnaireTxType.DuoxuantiErwei || tm.Tx == ExamQuestionnaireTxType.DanxuantiSanwei || tm.Tx == ExamQuestionnaireTxType.DuoxuantiSanwei)
                    {
                        var smallList = tmList.Where(small => small.ParentId == tm.Id).ToList();

                        if (smallList != null && smallList.Count > 0)
                        {
                            if (tm.Tx == ExamQuestionnaireTxType.DanxuantiSanwei || tm.Tx == ExamQuestionnaireTxType.DuoxuantiSanwei)
                            {
                                var dtlist = new List<List<string>>();
                                var online = new List<string>() { };

                                var tmOptions = ListUtils.ToList(tm.Get("options"));
                                if (tmOptions == null || tmOptions.Count == 0) continue;


                                online.Add("");
                                for (var i = 0; i < tmOptions.Count; i++)
                                {
                                    online.Add(tmOptions[i]);
                                    online.Add("");
                                }
                                dtlist.Add(online);

                                var yList = new List<string>();
                                var lastSmallTitle = "";

                                var optionCountList = new List<KeyValuePair<string, int>>();
                                var smallOptions = new List<string>();

                                foreach (var small in smallList)
                                {
                                    var smallid = small.Id;
                                    var smalltitle = small.Title;

                                    yList.Add(smalltitle);

                                    smallOptions = ListUtils.ToList(small.Get("options"));
                                    if (smallOptions == null || smallOptions.Count == 0) break;

                                    var smallcurent = 0;
                                    for (var j = 0; j < tmOptions.Count + 1; j++)
                                    {
                                        if (smallcurent < tmOptions.Count)
                                        {
                                            var twoline = new List<string>();
                                            if (lastSmallTitle == smalltitle)
                                            {
                                                twoline.Add("");
                                            }
                                            else
                                            {
                                                twoline.Add(smalltitle);
                                                lastSmallTitle = smalltitle;
                                            }

                                            for (var i = 0; i < tmOptions.Count; i++)
                                            {
                                                var keyword = $"{tmOptions[i]}_{smallOptions[smallcurent]}";

                                                twoline.Add(smallOptions[smallcurent]);
                                                var answerCount = await _questionnaireAnswerRepository.GetCountSubmitUser(0, 0, paper.Id, smallid, keyword);
                                                twoline.Add(answerCount.ToString());

                                                optionCountList.Add(new KeyValuePair<string, int>($"{smallid}{tmOptions[i]}{smallOptions[smallcurent]}", answerCount));
                                            }

                                            smallcurent++;
                                            dtlist.Add(twoline);
                                        }

                                    }

                                }

                                tm.Set("SmallList", dtlist);

                                var chatSeries = new List<Apexchart.Series_Data_Group>();
                                foreach (var smallOption in smallOptions)
                                {
                                    foreach (var optoin in tmOptions)
                                    {
                                        var yData = new Apexchart.Series_Data_Group()
                                        {
                                            Name = $"{optoin}-{smallOption}",
                                            Group = smallOption
                                        };
                                        var yDataList = new List<int>();
                                        foreach (var small in smallList)
                                        {
                                            var keyvalue = $"{small.Id}{optoin}{smallOption}";
                                            var answerCount = optionCountList.Find(q => q.Key == keyvalue).Value;
                                            yDataList.Add(answerCount);
                                        }
                                        yData.Data = yDataList;
                                        chatSeries.Add(yData);
                                    }
                                }


                                var chartOptions = Apexchart.GetChartOptions(yList);

                                tm.Set("ChatSeries", chatSeries);
                                tm.Set("ChartOptions", chartOptions);
                            }
                            else
                            {
                                var dtlist = new List<List<string>>();
                                var online = new List<string>() { };

                                var tmOptions = ListUtils.ToList(tm.Get("options"));
                                if (tmOptions == null || tmOptions.Count == 0) continue;


                                online.Add("");
                                for (var i = 0; i < tmOptions.Count; i++)
                                {
                                    online.Add(tmOptions[i]);
                                }
                                dtlist.Add(online);

                                var yList = new List<string>();
                                var optionCountList = new List<KeyValuePair<string, int>>();
                                foreach (var small in smallList)
                                {
                                    var smallid = small.Id;
                                    var smalltitle = small.Title;

                                    var twoline = new List<string>
                                    {
                                        smalltitle
                                    };
                                    yList.Add(smalltitle);

                                    for (var j = 0; j < tmOptions.Count; j++)
                                    {
                                        var keyword = $"{tmOptions[j]}";
                                        var answerCount = await _questionnaireAnswerRepository.GetCountSubmitUser(0, 0, paper.Id, smallid, keyword);
                                        twoline.Add(answerCount.ToString());
                                        optionCountList.Add(new KeyValuePair<string, int>($"{smalltitle}{tmOptions[j]}", answerCount));
                                    }
                                    dtlist.Add(twoline);
                                }
                                tm.Set("SmallList", dtlist);

                                var chatSeries = new List<Apexchart.Series_Data>();
                                for (var i = 0; i < tmOptions.Count; i++)
                                {
                                    var yData = new Apexchart.Series_Data()
                                    {
                                        Name = tmOptions[i]
                                    };
                                    var yDataList = new List<int>();
                                    foreach (var small in smallList)
                                    {
                                        var answerCount = optionCountList.Find(q => q.Key == $"{small.Title}{tmOptions[i]}").Value;
                                        yDataList.Add(answerCount);
                                    }
                                    yData.Data = yDataList;
                                    chatSeries.Add(yData);
                                }

                                var chartOptions = Apexchart.GetChartOptions(yList);

                                tm.Set("ChatSeries", chatSeries);
                                tm.Set("ChartOptions", chartOptions);
                            }
                        }
                    }
                    else
                    {
                        if (tm.ParentId == 0)
                        {
                            var tmAnswerToTal = 0;
                            var optionAnswer = new List<string>();
                            if (tm.Tx == ExamQuestionnaireTxType.Jiandati)
                            {
                                var answerList = await _questionnaireAnswerRepository.GetListAnswer(0, 0, paper.Id, tm.Id);
                                if (answerList != null && answerList.Count > 0)
                                {
                                    optionAnswer.AddRange(answerList);
                                }
                                tm.Set("OptionsAnswer", optionAnswer);
                            }
                            else
                            {
                                var optionsAnswers = new List<int>();
                                var optionsValues = new List<string>();
                                var options = ListUtils.ToList(tm.Get("options"));
                                if (options != null && options.Count > 0)
                                {
                                    for (var i = 0; i < options.Count; i++)
                                    {
                                        optionsAnswers.Add(0);
                                        var abcList = StringUtils.GetABC();
                                        var optionAnswerValue = abcList[i];
                                        optionsValues.Add(optionAnswerValue);
                                        var answerCount = await _questionnaireAnswerRepository.GetCountSubmitUser(0, 0, paper.Id, tm.Id, optionAnswerValue);
                                        optionsAnswers[i] = answerCount;
                                        tmAnswerToTal += answerCount;
                                    }
                                }
                                tm.Set("OptionsAnswer", optionsAnswers);
                                tm.Set("OptionsValues", optionsValues);
                            }
                            tm.Set("TmAnswerTotal", tmAnswerToTal);
                        }
                    }
                    tm.Set("TxName", tm.Tx.GetDisplayName());
                    tm.Set("TmIndex", tmIndex);
                }
            }

            paper.Set("TmTotal", tmTotal);
            return new GetResult
            {
                Item = paper,
                TmList = tmList
            };

        }
    }
}
