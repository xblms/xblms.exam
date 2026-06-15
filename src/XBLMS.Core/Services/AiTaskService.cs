using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    //启动服务 ollama serve
    //运行一个模型 ollama run gemma3
    //停止运行一个模型 ollama stop gemma3
    //移除一个模型 ollama rm gemma3
    //查看所有模型 ollama ls
    //查看当前运行的模型 ollama ps
    //1 安装 2运行一个模型 3启动服务
    public class AiTaskService : IAiTaskService
    {
        private readonly IConfigRepository _configRepository;
        private readonly ISettingsManager _settingsManager;

        public AiTaskService(IConfigRepository configRepository, ISettingsManager settingsManager)
        {
            _configRepository = configRepository;
            _settingsManager = settingsManager;
        }

        public async Task<(bool success, DoAI.DoAI_Version_Result result, string msg)> ExcutionStatus(string host)
        {
            var versionUrl = $"{host}/api/version";
            return await RestUtils.GetAsync<DoAI.DoAI_Version_Result>(versionUrl);
        }
        public async Task<(bool success, DoAI.DoAI_RunningModels_Result result, string msg)> ExcutionRunningModels(string host)
        {
            var aiUrl = $"{host}/api/ps";
            return await RestUtils.GetAsync<DoAI.DoAI_RunningModels_Result>(aiUrl);
        }
        public async Task<DoAI.DoAI_Tm_Result> ExcutionTm(ExamTx tx, string zsd)
        {
            var config = await _configRepository.GetAsync();
            var aiUrl = $"{config.AiHostUrl}/api/chat";
            var result = new DoAI.DoAI_Tm_Result()
            {
                Success = false,
                Msg = "无效的题型"
            };

            var (success, tmResultJson, msg) = await RestUtils.PostStringAsync(aiUrl, TmFormart(tx.ExamTxBase, config.AiRunningModel, zsd));
            tmResultJson = StringUtils.ReplaceNewline(tmResultJson, string.Empty);
            tmResultJson = StringUtils.Replace(tmResultJson, "*", string.Empty);
            tmResultJson = StringUtils.Replace(tmResultJson, " ", string.Empty);
            var tmResult = TranslateUtils.JsonDeserialize<DoAI.DoAI_Tm_AI_Result>(tmResultJson);
            result.Success = success;
            result.Msg = msg;
            if (success)
            {
                var tmError = false;
                var tmErrors = new List<string>();
                var tmContent = new DoAI.DoAI_Tm_AI_Result_Content();
                try
                {
                    tmContent = TranslateUtils.JsonDeserialize<DoAI.DoAI_Tm_AI_Result_Content>(tmResult.Message.Content);
                }
                catch
                {
                    tmErrors.Add("内容无法有结构的输出");
                    tmError = true;
                }
                var answer = tmContent.Answer;
                if (tx.ExamTxBase == ExamTxBase.Tiankongti)
                {
                    var tiankongCount = StringUtils.GetCount("___", tmContent.Content);
                    var answerList = new List<string>();
                    if (StringUtils.ContainsIgnoreCase(answer, ","))
                    {
                        answerList = ListUtils.GetStringList(answer, ",");
                    }
                    else if (StringUtils.ContainsIgnoreCase(answer, "，"))
                    {
                        answerList = ListUtils.GetStringList(answer, "，");
                    }
                    else if (StringUtils.ContainsIgnoreCase(answer, "；"))
                    {
                        answerList = ListUtils.GetStringList(answer, "；");
                    }
                    else if (StringUtils.ContainsIgnoreCase(answer, "、"))
                    {
                        answerList = ListUtils.GetStringList(answer, "、");
                    }
                    else
                    {
                        answerList = ListUtils.GetStringList(answer);
                    }
                    answer = ListUtils.ToString(answerList);
                    if (tiankongCount != answerList.Count)
                    {
                        tmError = true;
                        tmErrors.Add("填空和答案的数量不匹配");
                    }
                }
                if (string.IsNullOrEmpty(answer))
                {
                    tmError = true;
                    tmErrors.Add("答案是空的");
                }

                var tm = new ExamTmAi
                {
                    Title = tmContent.Content,
                    Answer = answer,
                    Jiexi = tmContent.Analysis,
                    Score = tx.Score,
                    Nandu = 1,
                    TxId = tx.Id,
                    Zhishidian = zsd,
                };

                tm.Set("TxName", tx.Name);
                tm.Set("BaseTx", tx.ExamTxBase.GetValue());
                if (tx.ExamTxBase == ExamTxBase.Panduanti)
                {
                    var options = new List<string>();
                    options.Add("正确");
                    options.Add("错误");
                    var optionsValues = new List<string>();
                    if (StringUtils.Contains(answer, "正确"))
                    {
                        optionsValues.Add("A");
                        optionsValues.Add("");
                        answer = "A";
                    }
                    else
                    {
                        answer = "B";
                        optionsValues.Add("");
                        optionsValues.Add("B");
                    }
                    tm.Answer = answer.ToUpper();
                    tm.Set("options", options);
                    tm.Set("optionsValues", optionsValues);

                }
                if (tx.ExamTxBase == ExamTxBase.Danxuanti || tx.ExamTxBase == ExamTxBase.Duoxuanti)
                {
                    tm.Answer = tm.Answer.ToUpper();
                    var options = tmContent.Option;
                    if (options != null && options.Count > 0)
                    {
                        options = ReplaceOptionStart(options);
                        var answers = new List<string>();
                        for (int optinindex = 0; optinindex < options.Count; optinindex++)
                        {
                            var option = options[optinindex];
                            answers.Add(StringUtils.GetABC()[optinindex]);
                            if (string.IsNullOrWhiteSpace(option))
                            {
                                break;
                            }
                        }
                        for (int answerIndex = 0; answerIndex < answers.Count; answerIndex++)
                        {
                            try
                            {
                                if (!ListUtils.ContainsIgnoreCase(tm.Answer,answers[answerIndex]))
                                {
                                    answers[answerIndex] = string.Empty;
                                }
                            }
                            catch { continue; }
                        }
                        if (options != null && options.Count > 0 && answers != null && answers.Count > 0)
                        {
                            if (options.Count != answers.Count && tx.ExamTxBase == ExamTxBase.Duoxuanti)
                            {
                                tmError = true;
                                tmErrors.Add("答案和选项数量不匹配");
                            }
                        }
                        else
                        {
                            tmErrors.Add("选项或者答案为空");
                            tmError = true;
                        }

                        tm.Set("options", options.ToArray());
                        tm.Set("optionsValues", answers.ToArray());
                        tm.Answer = string.Join(string.Empty, answers.ToArray());
                    }
                    tm.Title = StringUtils.Replace(tm.Title, "options", string.Empty);
                    if (!StringUtils.ContainsIgnoreCase(tm.Answer, "A") && !StringUtils.ContainsIgnoreCase(tm.Answer, "B") && !StringUtils.ContainsIgnoreCase(tm.Answer, "C") && !StringUtils.ContainsIgnoreCase(tm.Answer, "D") && !StringUtils.ContainsIgnoreCase(tm.Answer, "E") && !StringUtils.ContainsIgnoreCase(tm.Answer, "F"))
                    {
                        tmError = true;
                        tmErrors.Add("答案为空或者表述不正确，应该是连续的大写英文字母");
                    }
                    tm.Set("TmError", tmError);
                    tm.Set("TmErrors", tmErrors);
                }
                result.Tm = tm;
            }

            return result;
        }

        private static string TmFormart(ExamTxBase tx, string model, string zsd)
        {
            var json = string.Empty;
            if (tx == ExamTxBase.Tiankongti)
            {
                json = "{\"model\":\"aimodel\",\"messages\": [{\"role\": \"user\", \"content\": \"出1道关于aizsd的填空题,题目内容用content返回并且填空处用___填充,答案用answer返回多个答案用逗号分隔,题目解析用analysis返回,严格按照结构化要求返回json格式。\"}],\"stream\": false,\"format\": {\"type\": \"object\",\"properties\": {\"content\": {\"type\": \"string\"},\"answer\": {\"type\": \"string\"},\"analysis\": {\"type\": \"string\"}},\"required\": [\"content\",\"answer\",\"analysis\"]}}";
            }
            if (tx == ExamTxBase.Jiandati)
            {
                json = "{\"model\":\"aimodel\",\"messages\": [{\"role\": \"user\", \"content\": \"出1道关于aizsd的简答题，不能是选择题，不能是判断题，不能是填空题，附带答案，题目内容用content返回，答案用answer返回，题目解析用analysis返回，严格按照结构化要求返回json格式。\"}],\"stream\": false,\"format\": {\"type\": \"object\",\"properties\": {\"content\": {\"type\": \"string\"},\"answer\": {\"type\": \"string\"},\"analysis\": {\"type\": \"string\"}},\"required\": [\"content\",\"answer\",\"analysis\"]}}";
            }
            if (tx == ExamTxBase.Panduanti)
            {
                json = "{\"model\":\"aimodel\",\"messages\": [{\"role\": \"user\", \"content\": \"出1道关于aizsd的判断题，附带答案，答案只能是正确和错误，题目内容用content返回，答案用answer返回，题目解析用analysis返回，严格按照结构化要求返回json格式。\"}],\"stream\": false,\"format\": {\"type\": \"object\",\"properties\": {\"content\": {\"type\": \"string\"},\"answer\": {\"type\": \"string\"},\"analysis\": {\"type\": \"string\"}},\"required\": [\"content\",\"answer\",\"analysis\"]}}";
            }
            if (tx == ExamTxBase.Danxuanti)
            {
                json = "{\"model\":\"aimodel\",\"messages\": [{\"role\": \"user\", \"content\": \"出1道关于aizsd的单选题,题目内容用content返回,题目候选项必须是array用option返回,题目候选项不能出现在题目中并且必须以数组的形式放入option返回,答案只能包1个拉丁字母并且用answer返回,答案中不要带有除拉丁字母以外的任何符号,题目解析用analysis返回,严格按照结构化要求返回json格式。\"}],\"stream\": false,\"format\": {\"type\": \"object\",\"properties\": {\"content\": {\"type\": \"string\"},\"answer\": {\"type\": \"string\"},\"option\": {\"type\": \"array\"},\"analysis\": {\"type\": \"string\"}},\"required\": [\"content\",\"option\",\"answer\",\"analysis\"]}}";
            }
            if (tx == ExamTxBase.Duoxuanti)
            {
                json = "{\"model\":\"aimodel\",\"messages\": [{\"role\": \"user\", \"content\": \"出1道关于aizsd的多选题,题目内容用content返回,题目候选项必须是array用option返回,题目候选项不能出现在题目中并且必须以数组的形式放入option返回,答案必须是1个以上并且只能包拉丁字母并且用answer返回,答案中不要带有除拉丁字母以外的任何符号,题目解析用analysis返回,严格按照结构化要求返回json格式。\"}],\"stream\": false,\"format\": {\"type\": \"object\",\"properties\": {\"content\": {\"type\": \"string\"},\"answer\": {\"type\": \"string\"},\"option\": {\"type\": \"array\"},\"analysis\": {\"type\": \"string\"}},\"required\": [\"content\",\"option\",\"answer\",\"analysis\"]}}";
            }
            json = StringUtils.Replace(json, "aimodel", model);
            json = StringUtils.Replace(json, "aizsd", zsd);
            return json;
        }

        private static List<string> ReplaceOptionStart(List<string> options)
        {
            var newOptions = new List<string>();
            var abcList = StringUtils.GetABCSort();
            if (options != null && options.Count > 0)
            {
                foreach (var item in options)
                {
                    var newItem = item;
                    if (newItem.Length >= 2)
                    {
                        foreach (var item1 in abcList)
                        {
                            if (StringUtils.StartsWithIgnoreCase(newItem, item1))
                            {
                                newItem = newItem.Remove(0, 2);
                            }
                        }
                    }
                    newOptions.Add(newItem);
                }
            }
            return newOptions;
        }
    }
}
