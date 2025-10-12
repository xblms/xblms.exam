using Datory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireEditController
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
            var tmList = new List<ExamQuestionnaireTm>();

            var sheet = ExcelUtils.Read(filePath);

            if (sheet != null)
            {
                var tmTotal = sheet.Rows.Count - 2;
                if (tmTotal > 0)
                {
                    var parentId = 0;
                    var smallTx = ExamQuestionnaireTxType.Danxuanti;
                    var parentTx = ExamQuestionnaireTxType.Danxuanti;

                    for (var i = 1; i < sheet.Rows.Count; i++) //行
                    {
                        if (i == 1) continue;

                        var row = sheet.Rows[i];

                        var tx = row[0].ToString().Trim();
                        var title = row[1].ToString().Trim();

                        var rowIndexName = i + 1;

                        if (parentId > 0 && string.IsNullOrEmpty(tx))
                        {
                            if (string.IsNullOrEmpty(title))
                            {
                                errorMessageList.Add($"【行{rowIndexName}:小题的题目不能为空】");
                                break;
                            }

                            var tm = new ExamQuestionnaireTm();
                            tm.ParentId = parentId;
                            tm.Title = title;
                            tm.Tx = smallTx;

                            if (parentTx == ExamQuestionnaireTxType.DanxuantiSanwei || parentTx == ExamQuestionnaireTxType.DuoxuantiSanwei)
                            {
                                var options = new List<string>();
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
                                if (options.Count > 0)
                                {
                                    tm.Set("Options", options);
                                }
                                else
                                {
                                    errorMessageList.Add($"【行{rowIndexName}:三维小题的候选项不能为空】");
                                    break;
                                }
                            }
                            tmList.Add(tm);
                        }
                        else if (!string.IsNullOrEmpty(tx) && !string.IsNullOrEmpty(title))
                        {
                            var tm = new ExamQuestionnaireTm();
                            tm.Title = title;

                            if (tx == ExamQuestionnaireTxType.DanxuantiErwei.GetDisplayName() || tx == ExamQuestionnaireTxType.DuoxuantiErwei.GetDisplayName() || tx == ExamQuestionnaireTxType.DanxuantiSanwei.GetDisplayName() || tx == ExamQuestionnaireTxType.DuoxuantiSanwei.GetDisplayName())
                            {
                                tm.Id = i;
                                parentId = tm.Id;
                                if (tx == ExamQuestionnaireTxType.DanxuantiErwei.GetDisplayName())
                                {
                                    tm.Tx = parentTx = ExamQuestionnaireTxType.DanxuantiErwei;
                                    smallTx = ExamQuestionnaireTxType.Danxuanti;
                                }
                                if (tx == ExamQuestionnaireTxType.DuoxuantiErwei.GetDisplayName())
                                {
                                    tm.Tx = parentTx = ExamQuestionnaireTxType.DuoxuantiErwei;
                                    smallTx = ExamQuestionnaireTxType.Duoxuanti;
                                }
                                if (tx == ExamQuestionnaireTxType.DanxuantiSanwei.GetDisplayName())
                                {
                                    tm.Tx = parentTx = ExamQuestionnaireTxType.DanxuantiSanwei;
                                    smallTx = ExamQuestionnaireTxType.Danxuanti;
                                }
                                if (tx == ExamQuestionnaireTxType.DuoxuantiSanwei.GetDisplayName())
                                {
                                    tm.Tx = parentTx = ExamQuestionnaireTxType.DuoxuantiSanwei;
                                    smallTx = ExamQuestionnaireTxType.Duoxuanti;
                                }
                                var options = new List<string>();
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
                                tm.Set("Options", options);
                                tmList.Add(tm);
                            }
                            else
                            {
                                parentId = 0;
                                if (tx == ExamQuestionnaireTxType.Danxuanti.GetDisplayName())
                                {
                                    tm.Tx = ExamQuestionnaireTxType.Danxuanti;
                                }
                                else if (tx == ExamQuestionnaireTxType.Duoxuanti.GetDisplayName())
                                {
                                    tm.Tx = ExamQuestionnaireTxType.Duoxuanti;
                                }
                                else if (tx == ExamQuestionnaireTxType.Jiandati.GetDisplayName())
                                {
                                    tm.Tx = ExamQuestionnaireTxType.Jiandati;
                                }
                                else
                                {
                                    errorMessageList.Add($"【行{rowIndexName}:未知题型】");
                                    continue;
                                }

                                var options = new List<string>();
                                if (tm.Tx == ExamQuestionnaireTxType.Danxuanti || tm.Tx == ExamQuestionnaireTxType.Duoxuanti)
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
                                tmList.Add(tm);
                            }
                        }
                        else
                        {
                            errorMessageList.Add($"【行{rowIndexName}:题型和题目内容不能为空】");
                            break;
                        }
                    }
                }
                else
                {
                    errorMessageList.Add($"模板中没有编辑题目，请重新编辑模板文件后导入");
                }
            }

            FileUtils.DeleteFileIfExists(filePath);

            var tmIndex = 0;
            foreach (var item in tmList)
            {
                if (item.ParentId == 0) tmIndex++;
                if (item.Tx == ExamQuestionnaireTxType.DanxuantiErwei || item.Tx == ExamQuestionnaireTxType.DuoxuantiErwei || item.Tx == ExamQuestionnaireTxType.DanxuantiSanwei || item.Tx == ExamQuestionnaireTxType.DuoxuantiSanwei)
                {
                    var smallList = tmList.Where(tm => tm.ParentId == item.Id).ToList();
                    foreach (var small in smallList)
                    {
                        small.Set("Answer", "");
                        small.Set("Answers", new List<string>());
                    }
                    if (smallList != null && smallList.Count > 0)
                    {
                        item.Set("SmallList", smallList);
                    }
                }
                item.Set("TmIndex", tmIndex);
            }

            return new GetUploadTmResult
            {
                TmList = tmList,
                ErrorMsgList = errorMessageList,
                SuccessTotal = tmIndex
            };
        }

    }
}
