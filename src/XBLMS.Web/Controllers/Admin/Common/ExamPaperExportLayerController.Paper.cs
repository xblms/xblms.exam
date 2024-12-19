using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats.Vml;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
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
        [HttpGet, Route(Route)]
        public async Task<ActionResult<StringResult>> Export([FromQuery] GetReqest request)
        {
            var wordurl = string.Empty;

            var paper = await _examPaperRepository.GetAsync(request.PaperId);

            if (paper == null) { return NotFound(); }
            var randomIds = await _examPaperRandomRepository.GetIdsByPaperAsync(paper.Id);
            if (randomIds == null || randomIds.Count == 0) { return NotFound(); }
            var configs = await _examPaperRandomConfigRepository.GetListAsync(paper.Id);
            if (configs == null || configs.Count == 0) { return NotFound(); }

      
            if (request.Type == ExamPaperExportType.PaperOnlyOne)
            {
                wordurl = await ExportPaperOnlyOne(request, randomIds, paper, configs);
            }
            if (request.Type == ExamPaperExportType.PaperRar)
            {
                wordurl = await ExportPaperRar(request, randomIds, paper, configs);
            }

            return new StringResult
            {
                Value = wordurl
            };
        }
        public async Task<string> ExportPaperOnlyOne(GetReqest request, List<int> randomIds, ExamPaper paper, List<ExamPaperRandomConfig> configs)
        {

            var randomId = request.Id;
            if (randomId == 0)
            {
                randomId = randomIds[0];
            }

            var fileName = paper.Title;
            if (randomIds.Count > 1)
            {
                fileName = $"{paper.Title}(第{randomIds.IndexOf(request.Id) + 1}套)";
            }

            var htmlPath = _pathManager.GetTemporaryFilesPath("1.html");
            var fileHtmlPath = _pathManager.GetDownloadFilesPath($"{fileName}.html");
            FileUtils.CopyFile(htmlPath, fileHtmlPath);

            var wordFileName = $"{fileName}.docx";
            var wordPath = _pathManager.GetTemporaryFilesPath("1.docx");
            var fileWordPath = _pathManager.GetDownloadFilesPath($"{wordFileName}");

            FileUtils.CopyFile(wordPath, fileWordPath);

            var wordContent = new StringBuilder();
            wordContent.AppendFormat(@"<html><head></head><body>");

            wordContent.AppendFormat($"<div style='text-align:center;'><h1>{fileName}</h1></div>");

            var timing = string.Empty;
            if (paper.IsTiming)
            {
                timing = $"，答题时间{paper.TimingMinute}分钟";
            }
            wordContent.AppendFormat($"<div style='text-align:center;'>（共{paper.TmCount}题，总分{paper.TotalScore}，及格分{paper.PassScore}{timing}）</div>");

            wordContent.Append($"<p></p>");

            GetHead(wordContent, configs);

            wordContent.Append($"<p></p>");

            await GetTm(wordContent, configs, randomId, request.WithAnswer);

            StringUtils.ReplaceHrefOrSrc(wordContent, $"/{DirectoryUtils.SiteFiles.DirectoryName}/{DirectoryUtils.SiteFiles.Upload}", $"../{DirectoryUtils.SiteFiles.Upload}");

            await FileUtils.WriteTextAsync(fileHtmlPath, wordContent.ToString());
            var result = WordManager.HtmlToWord(fileHtmlPath, fileWordPath);

            var wordurl = _pathManager.GetDownloadFilesUrl(wordFileName);

            FileUtils.DeleteFileIfExists(fileHtmlPath);

            await _authManager.AddAdminLogAsync("导出试卷", fileName);
            await _authManager.AddStatLogAsync(StatType.Export, $"导出试卷({fileName})", 0, string.Empty, new StringResult { Value = wordurl });

            return wordurl;
        }
        public async Task<string> ExportPaperRar(GetReqest request, List<int> randomIds, ExamPaper paper, List<ExamPaperRandomConfig> configs)
        {

            var zipFileName = $"{paper.Title}-试卷打包({DateTime.Now.ToString("yyyyMMddHHmmss")})";
            var zipFilePath = _pathManager.GetDownloadFilesPath(zipFileName);

            DirectoryUtils.CreateDirectoryIfNotExists(zipFilePath);

            foreach (var randomId in randomIds)
            {
                var fileName = $"{paper.Title}(第{randomIds.IndexOf(randomId) + 1}套)";
                var htmlPath = _pathManager.GetTemporaryFilesPath("1.html");
                var fileHtmlPath = _pathManager.GetDownloadFilesPath($"{fileName}.html");
                FileUtils.CopyFile(htmlPath, fileHtmlPath);

                var wordFileName = $"{fileName}.docx";
                var wordPath = _pathManager.GetTemporaryFilesPath("1.docx");
                var fileWordPath = _pathManager.GetDownloadFilesPath(zipFileName, $"{wordFileName}");

                FileUtils.CopyFile(wordPath, fileWordPath);

                var wordContent = new StringBuilder();
                wordContent.AppendFormat(@"<html><head></head><body>");

                wordContent.AppendFormat($"<div style='text-align:center;'><h1>{fileName}</h1></div>");

                var timing = string.Empty;
                if (paper.IsTiming)
                {
                    timing = $"，答题时间{paper.TimingMinute}分钟";
                }
                wordContent.AppendFormat($"<div style='text-align:center;'>（共{paper.TmCount}题，总分{paper.TotalScore}，及格分{paper.PassScore}{timing}）</div>");

                wordContent.Append($"<p></p>");

                GetHead(wordContent, configs);

                wordContent.Append($"<p></p>");

                await GetTm(wordContent, configs, randomId, request.WithAnswer);

                StringUtils.ReplaceHrefOrSrc(wordContent, $"/{DirectoryUtils.SiteFiles.DirectoryName}/{DirectoryUtils.SiteFiles.Upload}", $"../../{DirectoryUtils.SiteFiles.Upload}");

                await FileUtils.WriteTextAsync(fileHtmlPath, wordContent.ToString());
                var result = WordManager.HtmlToWord(fileHtmlPath, fileWordPath);

                FileUtils.DeleteFileIfExists(fileHtmlPath);
            }

            var zipPath = $"{zipFilePath}.zip";

            _pathManager.CreateZip(zipPath, zipFilePath);
            DirectoryUtils.DeleteDirectoryIfExists(zipFilePath);

            var zipUrl = _pathManager.GetRootUrlByPath(zipPath);

            await _authManager.AddAdminLogAsync("打包试卷", paper.Title);
            await _authManager.AddStatLogAsync(StatType.Export, $"打包试卷({paper.Title})", 0, string.Empty, new StringResult { Value = zipUrl });

            return zipUrl;
        }
    }
}
