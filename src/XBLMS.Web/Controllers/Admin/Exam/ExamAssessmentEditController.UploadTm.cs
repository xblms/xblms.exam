using Datory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamAssessmentEditController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteUploadTm)]
        public async Task<ActionResult<GetUploadTmResult>> Import([FromForm] IFormFile file)
        {
            var admin = await _authManager.GetAdminAsync();

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
                return this.Error(error);
            }

            fileName = _pathManager.GetUploadFileName(fileName);
            var filePath = _pathManager.GetImportFilesPath(fileName);
            await _pathManager.UploadAsync(file, filePath);

            var errorMessageList = new List<string> { };
            var tmList = new List<ExamAssessmentTm>();

            var sheet = ExcelUtils.Read(filePath);


            if (sheet != null)
            {
                var tmTotal = sheet.Rows.Count - 2;
                if (tmTotal > 0)
                {

                    for (var i = 1; i < sheet.Rows.Count; i++) //行
                    {
                        if (i == 1) continue;


                        var row = sheet.Rows[i];

                        var tx = row[0].ToString().Trim();
                        var title = row[1].ToString().Trim();

                        var options = new List<string>();


                        var rowIndexName = i + 1;


                        if (!string.IsNullOrEmpty(tx) && !string.IsNullOrEmpty(title))
                        {
                            var tm = new ExamAssessmentTm();

                            tm.Title = title;
                            if (tx == ExamAssessmentTxType.Danxuanti.GetDisplayName())
                            {
                                tm.Tx = ExamAssessmentTxType.Danxuanti;
                            }
                            else if (tx == ExamAssessmentTxType.Duoxuanti.GetDisplayName())
                            {
                                tm.Tx = ExamAssessmentTxType.Duoxuanti;
                            }
                            else
                            {
                                errorMessageList.Add($"【行{rowIndexName}:未知题型】");
                                continue;
                            }

                            if (tm.Tx == ExamAssessmentTxType.Danxuanti || tm.Tx == ExamAssessmentTxType.Duoxuanti)
                            {
                                for (int optionindex = 2; optionindex < 17; optionindex++)
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
                            }
                            tm.Set("Options", options);

                            i++;
                            row = sheet.Rows[i];
                            var optionsScore = new List<int>();
                            for (int optionindex = 2; optionindex < 17; optionindex++)
                            {
                                try
                                {
                                    if (!string.IsNullOrWhiteSpace(row[optionindex].ToString().Trim()))
                                    {
                                        var optionValue = TranslateUtils.ToInt(row[optionindex].ToString().Trim());
                                        optionsScore.Add(optionValue);
                                    }
                                }
                                catch { }
                            }
                            tm.Set("OptionsScore", optionsScore);

                            tmList.Add(tm);
                        }
                        else
                        {
                            errorMessageList.Add($"【行{rowIndexName}:必填项不能为空】");
                        }
                    }
                }
                else
                {
                    errorMessageList.Add($"模板中没有编辑题目，请重新编辑模板文件后导入");
                }

            }

            FileUtils.DeleteFileIfExists(filePath);


            return new GetUploadTmResult
            {
                TmList = tmList,
                ErrorMsgList = errorMessageList
            };
        }

    }
}
