using Datory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using XBLMS.Core.Utils.Office;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireAnalysisController
    {
        [HttpGet, Route(RouteExportWord)]
        public async Task<ActionResult<StringResult>> ExportWord([FromQuery] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }

            var paper = await _questionnaireRepository.GetAsync(request.Id);
            var tmList = await _questionnaireTmRepository.GetListAsync(paper.Id);


            var fileName = $"问卷调查-{paper.Title}-统计结果-{DateTime.Now.ToString("yyyyMMddhhmmssSS")}";
            var htmlPath = _pathManager.GetTemporaryFilesPath("1.html");
            var fileHtmlPath = _pathManager.GetDownloadFilesPath($"{fileName}.html");
            FileUtils.CopyFile(htmlPath, fileHtmlPath);

            var wordFileName = $"{fileName}.docx";
            var wordPath = _pathManager.GetTemporaryFilesPath("1.docx");
            var fileWordPath = _pathManager.GetDownloadFilesPath($"{wordFileName}");

            FileUtils.CopyFile(wordPath, fileWordPath);


            var wordContent = new StringBuilder();
            wordContent.AppendFormat(@"<html><head></head><body>");

            wordContent.AppendFormat($"<div style='text-align:center;'><h2>{paper.Title}</h2></div>");
            wordContent.AppendFormat($"<div style='text-align:center;'>xblmtotal</div>");

            wordContent.Append($"<p></p>");
            wordContent.Append($"<p></p>");

            var tmTotal = 0;
            if (tmList != null && tmList.Count > 0)
            {
                var tmIndex = 1;
                foreach (var tm in tmList)
                {
                    wordContent.Append($"<p style='font-weight:bold;'>{tmIndex}、（{tm.Tx.GetDisplayName()}）{tm.Title}</p>");

                    tmTotal++;
                    var tmAnswerToTal = 0;
                    var optionAnswer = new List<string>();
                    if (tm.Tx == ExamQuestionnaireTxType.Jiandati)
                    {
                        var answerList = await _questionnaireAnswerRepository.GetListAnswer(paper.Id, tm.Id);
                        if (answerList != null && answerList.Count > 0)
                        {
                            foreach (var answer in answerList)
                            {
                                wordContent.Append($"<p>{ answer }</p>");
                            }
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
                                var answerCount = await _questionnaireAnswerRepository.GetCountSubmitUser(paper.Id, tm.Id, optionAnswerValue);
                                optionsAnswers[i] = answerCount;
                                tmAnswerToTal += answerCount;

                            }
                            wordContent.Append("<table style='border-collapse:collapse;border:0;width:100%;'>");
                            for (var i = 0; i < options.Count; i++)
                            {
                                var percent = TranslateUtils.ToPercent((decimal)optionsAnswers[i], (decimal)tmAnswerToTal);

                                wordContent.Append("<tr style='border:none;'>");
                                wordContent.Append($"<td style='border:none;padding:10px 5px;'>{StringUtils.GetABC()[i]}.{options[i]}</td><td style='padding:10px 5px;width:100px;border:none;'>{percent}%</td>");
                                wordContent.Append("</tr>");
                            }
                            wordContent.Append("</table>");
                        }
                        tm.Set("OptionsAnswer", optionsAnswers);
                        tm.Set("OptionsValues", optionsValues);
                    }
                    tm.Set("TmAnswerTotal", tmAnswerToTal);
                    tmIndex++;
                    wordContent.Append($"<p></p>");
                }
            }



            wordContent.AppendFormat(@"</body></html>");
            wordContent.Replace("xblmtotal", $"（共{tmTotal}项,调查人次：{paper.AnswerTotal}）");


            await FileUtils.WriteTextAsync(fileHtmlPath, wordContent.ToString());
            var result = WordManager.HtmlToWord(fileHtmlPath, fileWordPath);

            var wordurl = _pathManager.GetDownloadFilesUrl(wordFileName);

            FileUtils.DeleteFileIfExists(fileHtmlPath);

            await _authManager.AddAdminLogAsync("导出问卷调查统计结果", paper.Title);
            await _authManager.AddStatLogAsync(StatType.Export, "导出问卷调查统计结果", 0, string.Empty, new StringResult { Value = wordurl });

            return new StringResult
            {
                Value = wordurl
            };

        }
    }
}
