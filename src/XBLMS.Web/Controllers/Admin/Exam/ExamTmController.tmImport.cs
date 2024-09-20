using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
        [HttpGet, Route(RouteImportGetCache)]
        public async Task<ActionResult<CacheResultImportTm>> GetImportCache()
        {
            var admin = await _authManager.GetAdminAsync();


            //进度缓存
            var cacheKey = CacheUtils.GetEntityKey("tmImport", admin.Id);
            var cache = _cacheManager.Get<CacheResultImportTm>(cacheKey);
            if (cache != null)
            {
                if (cache.IsOver || cache.IsError)
                {
                    cache.IsStop = true;
                    _cacheManager.Remove(cacheKey);
                }
            }
            else
            {
                return new CacheResultImportTm();
            }
            return cache;
        }


        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteImportExcel)]
        public async Task<ActionResult<GetImportResult>> Import([FromQuery] int treeId, [FromForm] IFormFile file)
        {
            var admin = await _authManager.GetAdminAsync();

            var cacheKey = CacheUtils.GetEntityKey("tmImport", admin.Id);
            var cacheInfo = new CacheResultImportTm() { IsError = false, IsOver = false, Errors = new List<string>() };
            _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);

            var companyId = admin.CompanyId;

            if (file == null)
            {
                return this.Error(Constants.ErrorUpload);
            }

            var fileName = PathUtils.GetFileName(file.FileName);

            var errors = new List<string>();

            var sExt = PathUtils.GetExtension(fileName);
            if (!StringUtils.EqualsIgnoreCase(sExt, ".xlsx"))
            {

                var error = "导入文件为xlsx格式，请选择有效的文件上传";

                cacheInfo.IsOver = true;
                cacheInfo.IsError = true;
                cacheInfo.Errors.Add(error);
                _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);

                return this.Error(error);
            }

            fileName = _pathManager.GetUploadFileName(fileName);
            var filePath = _pathManager.GetImportFilesPath(fileName);
            await _pathManager.UploadAsync(file, filePath);

            var errorMessage = string.Empty;
            var success = 0;
            var failure = 0;
            var errorMessageList = new List<ImportMessage> { };
            var sheet = ExcelUtils.Read(filePath);


            if (sheet != null)
            {
                var tmTotal = sheet.Rows.Count - 2;
                if (tmTotal > 0)
                {
                    cacheInfo.TmTotal = tmTotal;
                    cacheInfo.TmCurrent = 0;
                    _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);

                    for (var i = 1; i < sheet.Rows.Count; i++) //行
                    {
                        if (i == 1) continue;

                        //Thread.Sleep(100);

                        var row = sheet.Rows[i];

                        var tx = row[0].ToString().Trim();
                        var nanduString = row[1].ToString().Trim();
                        var nandu = 1;
                        try
                        {
                            nandu = Convert.ToInt32(nanduString);
                        }
                        catch { }

                        var zhishidian = row[2].ToString().Trim();
                        var score = row[3].ToString().Trim();
                        decimal scoreDouble = 0;

                        var jiexi = row[4].ToString().Trim();
                        var title = row[5].ToString().Trim();
                        var answer = row[6].ToString().Trim().ToUpper();
                        var options = new List<string>();


                        var rowIndexName = i + 1;


                        if (!string.IsNullOrEmpty(tx) && !string.IsNullOrEmpty(title))
                        {

                            var txInfo = await _examTxRepository.GetAsync(tx);
                            if (txInfo == null)
                            {
                                cacheInfo.TmCurrent++;
                                cacheInfo.Errors.Add($"【行{rowIndexName}:无效的题型】");
                                _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);

                                failure++;
                                continue;
                            }
                            if (txInfo.ExamTxBase != ExamTxBase.Tiankongti && txInfo.ExamTxBase != ExamTxBase.Jiandati)
                            {
                                for (int optionindex = 7; optionindex < 17; optionindex++)
                                {
                                    try
                                    {
                                        if (!string.IsNullOrWhiteSpace(row[optionindex].ToString().Trim()))
                                        {
                                            options.Add(row[optionindex].ToString().Trim());
                                        }
                                    }
                                    catch { }
                                }
                                if (options.Count < answer.Length)
                                {
                                    cacheInfo.TmCurrent++;
                                    cacheInfo.Errors.Add($"【行{rowIndexName}:候选项和答案不匹配】");

                                    _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);
                                    failure++;
                                    continue;
                                }
                            }
                            if (txInfo.ExamTxBase == ExamTxBase.Tiankongti)
                            {
                                if (!StringUtils.Contains(title, "___"))
                                {
                                    cacheInfo.TmCurrent++;
                                    cacheInfo.Errors.Add($"【行{rowIndexName}:填空题未包含___】"); 

                                    _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);

                                    failure++;
                                    continue;
                                }
                            }

                            try
                            {
                                var examInfo = new ExamTm
                                {
                                    TreeId = treeId,
                                    TxId = txInfo.Id,
                                    Title = title,
                                    Answer = answer,
                                    Score = scoreDouble,
                                    Zhishidian = zhishidian,
                                    Nandu = nandu,
                                    Jiexi = jiexi,
                                    CompanyId = companyId,
                                    CreatorId = admin.Id,
                                    DepartmentId = admin.DepartmentId
                                };

                                if (options.Count > 0)
                                {
                                    var answers = new List<string>();
                                    for (int optinindex = 0; optinindex < options.Count; optinindex++)
                                    {
                                        answers.Add(StringUtils.GetABC()[optinindex]);
                                        if (string.IsNullOrWhiteSpace(options[optinindex]))
                                        {
                                            cacheInfo.TmCurrent++;
                                            cacheInfo.Errors.Add($"【行{rowIndexName}:候选项内容不完整】");

                                            _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);
                                            failure++;
                                            continue;
                                        }
                                    }
                                    for (int answerIndex = 0; answerIndex < answers.Count; answerIndex++)
                                    {
                                        if (!answer.Contains(answers[answerIndex]))
                                        {
                                            answers[answerIndex] = "";
                                        }
                                    }
                                    examInfo.Set("options", options.ToArray());
                                    examInfo.Set("optionsValues", answers.ToArray());
                                }

                                if (await _examTmRepository.ExistsAsync(examInfo.Title, examInfo.TxId))
                                {
                                    cacheInfo.TmCurrent++;
                                    cacheInfo.Errors.Add($"【行{rowIndexName}:题库中已经存在相同题型的题目，请勿重复导入】");

                                    _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);
                                    failure++;
                                }
                                else
                                {
                                    var tmid = await _examTmRepository.InsertAsync(examInfo);

                                    if (tmid > 0)
                                    {
                                        await _statRepository.AddCountAsync(StatType.ExamTmAdd);
                                        success++;

                                        cacheInfo.TmCurrent++;
                                        _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);
                                    }
                                    else
                                    {
                                        cacheInfo.TmCurrent++;
                                        cacheInfo.Errors.Add($"【行{rowIndexName}:题目导入失败】");

                                        _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);
                                        failure++;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                failure++;
                                cacheInfo.TmCurrent++;
                                cacheInfo.Errors.Add($"【行{rowIndexName}:{ex.Message}】");

                                _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);

                            }
                        }
                        else
                        {
                            cacheInfo.TmCurrent++;
                            cacheInfo.Errors.Add($"【行{rowIndexName}:必填项不能为空】");

                            _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);

                            failure++;
                        }
                    }
                }
                else
                {
                    cacheInfo.IsOver = true;
                    cacheInfo.IsError = true;
                    cacheInfo.Errors.Add("模板中没有编辑题目，请重新编辑模板文件后导入");

                    _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);
                }

            }

            FileUtils.DeleteFileIfExists(filePath);

            cacheInfo.IsOver = true;
            _cacheManager.AddOrUpdateAbsolute(cacheKey, cacheInfo, 1);


            return new GetImportResult
            {
                Value = true,
                Success = success,
                Failure = failure,
                ErrorMessage = errorMessage,
                ErrorMessageList = errorMessageList
            };
        }


    }
}
