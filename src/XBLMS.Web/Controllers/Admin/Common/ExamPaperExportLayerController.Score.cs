using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Core.Utils.Office;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class ExamPaperExportLayerController
    {
        [HttpGet, Route(RouteScore)]
        public async Task<ActionResult<StringResult>> ExportScore([FromQuery] GetReqest request)
        {
            var wordurl = string.Empty;

            var paper = await _examPaperRepository.GetAsync(request.PaperId);

            if (paper == null) { return NotFound(); }
            var randomIds = await _examPaperRandomRepository.GetIdsByPaperAsync(paper.Id);
            if (randomIds == null || randomIds.Count == 0) { return NotFound(); }
            var configs = await _examPaperRandomConfigRepository.GetListAsync(paper.Id);
            if (configs == null || configs.Count == 0) { return NotFound(); }


            if (request.Type == ExamPaperExportType.PaperScoreOnlyOne)
            {
                wordurl = await ExportScoreOnlyOne(request, randomIds, paper, configs);
            }
            if (request.Type == ExamPaperExportType.PaperScoreRar)
            {
                wordurl = await ExportScorerRar(request, randomIds, paper, configs);
            }

            return new StringResult
            {
                Value = wordurl
            };
        }
        public async Task<string> ExportScoreOnlyOne(GetReqest request, List<int> randomIds, ExamPaper paper, List<ExamPaperRandomConfig> configs)
        {
            var start = await _examPaperStartRepository.GetAsync(request.Id);
            var user = await _userRepository.GetByUserIdAsync(start.UserId);
            await _organManager.GetUser(user);

            var randomId = start.ExamPaperRandomId;

            var fileName = $"{paper.Title}({user.DisplayName}-答卷-{start.EndDateTime.Value.ToString(DateUtils.FormatStringDateTimeWithOutSpace)})";

            var htmlPath = _pathManager.GetTemporaryFilesPath("1.html");
            var fileHtmlPath = _pathManager.GetDownloadFilesPath($"{fileName}.html");
            FileUtils.CopyFile(htmlPath, fileHtmlPath);

            var wordFileName = $"{fileName}.docx";
            var wordPath = _pathManager.GetTemporaryFilesPath("1.docx");
            var fileWordPath = _pathManager.GetDownloadFilesPath($"{wordFileName}");

            FileUtils.CopyFile(wordPath, fileWordPath);

            var wordContent = new StringBuilder();
            wordContent.AppendFormat(@"<html><head></head><body>");

            wordContent.AppendFormat($"<div style='text-align:center;'><h1>{paper.Title}</h1></div>");

            var timing = string.Empty;
            if (paper.IsTiming)
            {
                timing = $"，答题时间{paper.TimingMinute}分钟";
            }
            wordContent.AppendFormat($"<div style='text-align:center;'>（共{paper.TmCount}题，总分{paper.TotalScore}，及格分{paper.PassScore}{timing}）</div>");


            wordContent.AppendFormat($"<div style='text-align:center;'>考生：{user.DisplayName}（{user.UserName}，{user.Get("OrganNames")}）</div>");
            wordContent.AppendFormat($"<div style='text-align:center;'>成绩：{start.Score}，用时：{DateUtils.SecondToHmsCN(start.ExamTimeSeconds)}</div>");
            wordContent.Append($"<p></p>");


            wordContent.Append("<table style='width:100%;border-collapse:collapse;'>");
            wordContent.Append("<tr>");
            wordContent.Append($"<td style='padding:8px;text-align:center;font-weight:bold;border:1px solid #000000;'>题号</td>");
            for (var i = 0; i < configs.Count; i++)
            {
                wordContent.Append($"<td style='padding:8px;text-align:center;font-weight:bold;border:1px solid #000000;'>{StringUtils.ParseNumberToChinese(i + 1)}</td>");
            }
            wordContent.Append($"<td style='padding:8px;text-align:center;font-weight:bold;border:1px solid #000000;'>总分</td>");
            wordContent.Append("</tr>");
            wordContent.Append("<tr>");
            wordContent.Append($"<td style='padding:8px;text-align:center;font-weight:bold;border:1px solid #000000;'>得分</td>");
            for (var i = 0; i < configs.Count; i++)
            {
                decimal txTotalScore = 0;
                var tms = await _examPaperRandomTmRepository.GetListAsync(randomId, configs[i].TxId, paper.Id);
                if (tms != null && tms.Count > 0)
                {
                    foreach (var item in tms)
                    {
                        var answer = await _examPaperAnswerRepository.GetAsync(item.Id, start.Id, paper.Id);
                        txTotalScore += answer.Score;
                    }
                }
                wordContent.Append($"<td style='padding:8px;text-align:center;border:1px solid #000000;'>{txTotalScore}</td>");
            }
            wordContent.Append($"<td style='padding:8px;text-align:center;border:1px solid #000000;'>{start.Score}</td>");
            wordContent.Append("</tr>");
            wordContent.Append("</table>");


            wordContent.Append($"<p></p>");

            await GetTm(wordContent, configs, randomId, request.WithAnswer, start, paper, true);

            StringUtils.ReplaceHrefOrSrc(wordContent, $"/{DirectoryUtils.SiteFiles.DirectoryName}/{DirectoryUtils.SiteFiles.Upload}", $"../{DirectoryUtils.SiteFiles.Upload}");

            await FileUtils.WriteTextAsync(fileHtmlPath, wordContent.ToString());
            var result = WordManager.HtmlToWord(fileHtmlPath, fileWordPath);

            var wordurl = _pathManager.GetDownloadFilesUrl(wordFileName);

            FileUtils.DeleteFileIfExists(fileHtmlPath);

            await _authManager.AddAdminLogAsync("导出答卷", fileName);
            await _authManager.AddStatLogAsync(StatType.Export, $"导出答卷({fileName})", 0, string.Empty, new StringResult { Value = wordurl });

            return wordurl;
        }
        public async Task<string> ExportScorerRar(GetReqest request, List<int> randomIds, ExamPaper paper, List<ExamPaperRandomConfig> configs)
        {

            var zipFileName = $"{paper.Title}-答卷打包({DateTime.Now.ToString("yyyyMMddHHmmss")})";
            var zipFilePath = _pathManager.GetDownloadFilesPath(zipFileName);

            DirectoryUtils.CreateDirectoryIfNotExists(zipFilePath);

            var (total, list) = await _examPaperStartRepository.GetListByAdminAsync(request.PaperId, 0, 0, request.DateFrom, request.DateTo, request.Keywords, 1, int.MaxValue);
            foreach (var start in list)
            {
                var user = await _userRepository.GetByUserIdAsync(start.UserId);
                await _organManager.GetUser(user);

                var fileName = $"{paper.Title}({user.DisplayName}-答卷-{start.EndDateTime.Value.ToString(DateUtils.FormatStringDateTimeWithOutSpace)})";

                var htmlPath = _pathManager.GetTemporaryFilesPath("1.html");
                var fileHtmlPath = _pathManager.GetDownloadFilesPath($"{fileName}.html");
                FileUtils.CopyFile(htmlPath, fileHtmlPath);

                var wordFileName = $"{fileName}.docx";
                var wordPath = _pathManager.GetTemporaryFilesPath("1.docx");
                var fileWordPath = _pathManager.GetDownloadFilesPath(zipFileName, $"{wordFileName}");

                FileUtils.CopyFile(wordPath, fileWordPath);

                var wordContent = new StringBuilder();
                wordContent.AppendFormat(@"<html><head></head><body>");

                wordContent.AppendFormat($"<div style='text-align:center;'><h1>{paper.Title}</h1></div>");

                var timing = string.Empty;
                if (paper.IsTiming)
                {
                    timing = $"，答题时间{paper.TimingMinute}分钟";
                }
                wordContent.AppendFormat($"<div style='text-align:center;'>（共{paper.TmCount}题，总分{paper.TotalScore}，及格分{paper.PassScore}{timing}）</div>");


                wordContent.AppendFormat($"<div style='text-align:center;'>考生：{user.DisplayName}（{user.UserName}，{user.Get("OrganNames")}）</div>");
                wordContent.AppendFormat($"<div style='text-align:center;'>成绩：{start.Score}，用时：{DateUtils.SecondToHmsCN(start.ExamTimeSeconds)}</div>");

                wordContent.Append($"<p></p>");

                wordContent.Append("<table style='width:100%;border-collapse:collapse;'>");
                wordContent.Append("<tr>");
                wordContent.Append($"<td style='padding:8px;text-align:center;font-weight:bold;border:1px solid #000000;'>题号</td>");
                for (var i = 0; i < configs.Count; i++)
                {
                    wordContent.Append($"<td style='padding:8px;text-align:center;font-weight:bold;border:1px solid #000000;'>{StringUtils.ParseNumberToChinese(i + 1)}</td>");
                }
                wordContent.Append($"<td style='padding:8px;text-align:center;font-weight:bold;border:1px solid #000000;'>总分</td>");
                wordContent.Append("</tr>");
                wordContent.Append("<tr>");
                wordContent.Append($"<td style='padding:8px;text-align:center;font-weight:bold;border:1px solid #000000;'>得分</td>");
                for (var i = 0; i < configs.Count; i++)
                {
                    decimal txTotalScore = 0;
                    var tms = await _examPaperRandomTmRepository.GetListAsync(start.ExamPaperRandomId, configs[i].TxId, paper.Id);
                    if (tms != null && tms.Count > 0)
                    {
                        foreach (var item in tms)
                        {
                            var answer = await _examPaperAnswerRepository.GetAsync(item.Id, start.Id, paper.Id);
                            txTotalScore += answer.Score;
                        }
                    }
                    wordContent.Append($"<td style='padding:8px;text-align:center;border:1px solid #000000;'>{txTotalScore}</td>");
                }
                wordContent.Append($"<td style='padding:8px;text-align:center;border:1px solid #000000;'>{start.Score}</td>");
                wordContent.Append("</tr>");
                wordContent.Append("</table>");

                wordContent.Append($"<p></p>");

                await GetTm(wordContent, configs, start.ExamPaperRandomId, request.WithAnswer, start, paper, true);

                StringUtils.ReplaceHrefOrSrc(wordContent, $"/{DirectoryUtils.SiteFiles.DirectoryName}/{DirectoryUtils.SiteFiles.Upload}", $"../../{DirectoryUtils.SiteFiles.Upload}");

                await FileUtils.WriteTextAsync(fileHtmlPath, wordContent.ToString());
                var result = WordManager.HtmlToWord(fileHtmlPath, fileWordPath);

                FileUtils.DeleteFileIfExists(fileHtmlPath);
            }

            var zipPath = $"{zipFilePath}.zip";

            _pathManager.CreateZip(zipPath, zipFilePath);
            DirectoryUtils.DeleteDirectoryIfExists(zipFilePath);

            var zipUrl = _pathManager.GetRootUrlByPath(zipPath);

            await _authManager.AddAdminLogAsync("打包答卷", paper.Title);
            await _authManager.AddStatLogAsync(StatType.Export, $"打包答卷({paper.Title})", 0, string.Empty, new StringResult { Value = zipUrl });

            return zipUrl;
        }
    }
}
