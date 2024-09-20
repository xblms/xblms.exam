using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Core.Utils.Office;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmController
    {

        [HttpPost, Route(RouteExportWord)]
        public async Task<ActionResult<StringResult>> WordExport([FromBody] GetSearchRequest request)
        {
            var admin = await _authManager.GetAdminAsync();
 

            //var lmsUtils = new LmsUtils(_databaseManager);
            //var purviewParams = await lmsUtils.GetPurviewParamsAsync(admin);


            //var fileName = _pathManager.Xlms_GetRandomFileName("1.txt");

            //var htmlPath = _pathManager.GetUploadTemporaryFilesPath("1.html");
            //var fileHtmlPath = _pathManager.Xlms_GetDownLoadPath(fileName, organId.ToString());


            //var wordfileName = _pathManager.Xlms_GetRandomFileName("1.docx");
            //var wordPath = _pathManager.GetUploadTemporaryFilesPath("1.docx");

            //var fileWordPath = _pathManager.Xlms_GetDownLoadPath(wordfileName, organId.ToString());

            ////await FileUtils.WriteTextAsync(fileHtmlPath, "");
            //FileUtils.CopyFile(wordPath, fileWordPath);
            //var (total, tmList) = await _examTmRepository.GetSearchAsync(purviewParams, organId, request.TkId, request.TxId, request.Difficulty, request.Keyword, request.Order, request.OrderType, request.IsStop, 1, int.MaxValue);
            //var wordContent = new StringBuilder();
            //wordContent.AppendFormat(@"<html><head></head><body>");
            //foreach (var tm in tmList)
            //{
            //    wordContent.Append($"<p>{tm.Title.Replace("<p>", "").Replace("</p>", "")}</p>");
            //    var tx = await _examTxRepository.GetAsync(tm.TxId, organId);
            //    if (tx.TxType == ExamTxType.Single || tx.TxType == ExamTxType.Multiple || tx.TxType == ExamTxType.judge)
            //    {
            //        var options = ListUtils.ToList(tm.Get("options"));
            //        var optionIndex = 0;
            //        foreach (var o in options)
            //        {
            //            wordContent.Append($"<p>{ExamUtils.GetABC()[optionIndex]}、{o}</p>");
            //            optionIndex++;
            //        }
            //    }
            //    wordContent.Append($"<p>标注：{tm.Answer}，{tx.Name}，{ExamUtils.GetDifficultyDisplayName(tm.Difficulty)}，{tm.Knowledge}，{tm.Analysis}，{tm.Score}</p>");
            //    wordContent.Append($"<p></p>");
            //}
            //wordContent.AppendFormat(@"</body></html>");

            //StringUtils.ReplaceHrefOrSrc(wordContent, $"/{DirectoryUtils.UploadFiles.DirectoryName}/{organId}/", $"../");
            //await FileUtils.WriteTextAsync(fileHtmlPath, wordContent.ToString());
            //var result = WordManager.SetTmWord(fileHtmlPath, fileWordPath);
            //var wordurl = _pathManager.GetRootUrlByPath(result);
            return new StringResult
            {
                Value = "wordurl"
            };
        }


        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteImportWord)]
        public async Task<ActionResult<GetImportWordResult>> WordImport([FromQuery] int tkId, [FromForm] IFormFile file)
        {
            var admin = await _authManager.GetAdminAsync();
            return null;

            //    var companyId = admin.CompanyId;
            //    var organId = admin.OrganId;
            //    if (file == null)
            //    {
            //        return this.Error(Constants.ErrorUpload);
            //    }
            //   // var config = await _companyRepository.GetAsync(organId);
            //    var fileName = PathUtils.GetFileName(file.FileName);

            //    var sExt = PathUtils.GetExtension(fileName);
            //    if (!StringUtils.EqualsIgnoreCase(sExt, ".docx"))
            //    {
            //        return this.Error("导入文件为docx格式，请选择有效的文件上传");
            //    }
            //    var randomName = _pathManager.Xlms_GetRandomFileName(fileName);
            //    var filePath = _pathManager.Xlms_GetUploadPath(randomName, organId.ToString());
            //    await _pathManager.UploadAsync(file, filePath);


            //    string imageDirectoryPath;
            //    string imageDirectoryUrl;
            //    imageDirectoryPath = _pathManager.Xlms_GetWordImgPath(organId.ToString());
            //    imageDirectoryUrl = _pathManager.Xlms_GetWordImgUrl(organId.ToString());
            //    var wordManager = new WordManager(imageDirectoryPath, imageDirectoryUrl);


            //    var wordContent = await wordManager.ConvertToHtmlAsync(filePath);
            //    wordContent = HtmlUtils.ClearFormat(wordContent);
            //    if (string.IsNullOrEmpty(wordContent))
            //    {
            //        return this.Error("文件格式有误或内容为空，请选择有效的文件上传");
            //    }
            //    else
            //    {
            //        List<string> errorMsg = new List<string>();
            //        var okcount = 0;
            //        var nocount = 0;



            //        var examUtils = new ExamUtils(_databaseManager);
            //        var (isSuccess, msg, wordTmList) = await examUtils.GetTmListFromWord(wordContent, organId);
            //        if (isSuccess)
            //        {
            //            foreach (var tm in wordTmList)
            //            {
            //                //if (config.CloudCountTm <= 0 && !admin.IsAdmin) break;
            //                tm.TkId = tkId;
            //                tm.CompanyId = admin.CompanyId;
            //                tm.OrganId = admin.OrganId;
            //                tm.CreatedUserId = admin.Id;
            //                tm.CreatedUserName = admin.UserName;
            //                tm.DepartmentId = admin.DepartmentId;
            //                var tmid = await _examTmRepository.InsertAsync(tm);
            //                if (tmid > 0)
            //                {
            //                    //config.CloudCountTm--;
            //                    await _statRepository.AddCountAsync(StatType.TmAdd, admin.DepartmentId, companyId, organId, admin.Id);
            //                    okcount++;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            errorMsg.Add(msg);
            //            nocount++;
            //        }
            //        //if (!admin.IsAdmin)
            //        //{
            //        //    await _companyRepository.UpdateAsync(config);
            //        //}
            //        FileUtils.DeleteFileIfExists(filePath);
            //        return new GetImportWordResult { OkCount = okcount, NoCount = nocount, ErrorList = errorMsg };
            //    }

            //}

        }
    }
}
