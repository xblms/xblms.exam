using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class EditorTmWordOpenLayerController : ControllerBase
    {
        private const string Route = "common/editorTmWordOpenLayer";
        private const string RouteCheck = Route + "/check";
        private const string RouteSubmit = Route + "/submit";

        private readonly IHttpContextAccessor _context;
        private readonly IAuthManager _authManager;
        private readonly IExamTmRepository _examTmRepository;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IStatRepository _statRepository;
        private readonly IExamTmTreeRepository _examTmTreeRepository;
        private readonly ITableStyleRepository _tableStyleRepository;

        public EditorTmWordOpenLayerController(IHttpContextAccessor context, IAuthManager authManager, IExamTmRepository examTmRepository, IExamTxRepository examTxRepository, IStatRepository statRepository, IExamTmTreeRepository examTmTreeRepository, ITableStyleRepository tableStyleRepository)
        {
            _context = context;
            _authManager = authManager;
            _examTmRepository = examTmRepository;
            _examTxRepository = examTxRepository;
            _statRepository = statRepository;
            _examTmTreeRepository = examTmTreeRepository;
            _tableStyleRepository = tableStyleRepository;
        }

        public class GetResult
        {
            public string Example { get; set; }
        }
        public class GetRequest
        {
            public int TreeId { get; set; }
            public string TmHtml { get; set; }
        }

        public class GetCheckResult
        {
            public int Total { get; set; }
            public int ErrorTotal { get; set; }
            public int SuccessTotal { get; set; }
            public string ResultHtml { get; set; }
        }

        private class CheckErrorMsg
        {
            public string TmHtml { get; set; }
            public List<string> ErrorMsg { get; set; }
        }


        public async Task<(int total, int successTotal, int errorTotal, List<ExamTm> successTmList, string resultTmHtml)> Check(string tmHtml, int treeId, Administrator admin, AdminAuth adminAuth)
        {
            var total = 0;
            var successTotal = 0;
            var errorTotal = 0;

            var successTmList = new List<ExamTm>();
            var errorTms = new List<CheckErrorMsg>();


            var regFront = @"<xblm>(.*?)</xblm>";
            tmHtml = Regex.Replace(tmHtml, regFront, string.Empty, RegexOptions.IgnoreCase);
            tmHtml = HtmlUtils.ClearFormat(tmHtml);//清理样式和p之外的标签
            tmHtml = StringUtils.StripBlank(tmHtml);//清理多余的空格

            var resultTmHtml = string.Empty;

            if (!string.IsNullOrEmpty(tmHtml))
            {
                var tmhtmlList = ListUtils.GetStringList(tmHtml, "<p><br/></p>");
                ListUtils.RemoveIgnoreCase(tmhtmlList, "");
                ListUtils.RemoveIgnoreCase(tmhtmlList, "<p><br/></p>");

                if (tmhtmlList != null && tmhtmlList.Count > 0)
                {
                    foreach (var tmhtml in tmhtmlList)
                    {
                        var testTmHtml = StringUtils.StripTags(tmhtml);
                        if (!string.IsNullOrEmpty(tmhtml) && !string.IsNullOrEmpty(testTmHtml))
                        {
                            total++;

                            var errorTm = new CheckErrorMsg();
                            errorTm.ErrorMsg = new List<string>();
                            errorTm.TmHtml = tmhtml;

                            if (tmhtml.Contains("题目注释"))
                            {
                                var newTmHtml = StringUtils.ReplaceEndsWithIgnoreCase(tmhtml, "<p></p>", string.Empty);
                                newTmHtml = StringUtils.ReplaceIgnoreCase(newTmHtml, "</p><p>", "</p>xblm<p>");//把每道题用xblm分割
                                var tmTitle_option_remark_list = ListUtils.GetStringList(newTmHtml, "xblm");


                                if (tmTitle_option_remark_list.Count >= 2)
                                {
                                    var tmTitle = tmTitle_option_remark_list[0];
                                    tmTitle = StringUtils.StripTags(tmTitle);

                                    var tmRemark = tmTitle_option_remark_list[tmTitle_option_remark_list.Count - 1];
                                    tmRemark = StringUtils.StripTags(tmRemark);
                                    var tmRemarkList = ListUtils.GetStringList(tmRemark, "|");

                                    if (tmRemarkList != null && tmRemarkList.Count >= 7)
                                    {
                                        var htmlTx = tmRemarkList[1];
                                        var htmlAnswer = tmRemarkList[2];
                                        var htmlScore = tmRemarkList[3];
                                        var htmlNd = tmRemarkList[4];
                                        var htmlZsd = tmRemarkList[5];
                                        var htmlJx = tmRemarkList[6];


                                        var options = new List<string>();
                                        var answers = new List<string>();

                                        var tx = await _examTxRepository.GetAsync(htmlTx);
                                        if (tx == null)
                                        {
                                            errorTotal++;
                                            errorTm.ErrorMsg.Add("无效的题型");
                                        }
                                        else
                                        {
                                            var checkOptions = true;
                                            var checkTitankongti = true;
                                            if (tx.ExamTxBase == ExamTxBase.Tiankongti || tx.ExamTxBase == ExamTxBase.Jiandati)
                                            {
                                                if (tx.ExamTxBase == ExamTxBase.Tiankongti)
                                                {
                                                    if (!StringUtils.Contains(tmTitle, "___"))
                                                    {
                                                        errorTotal++;
                                                        errorTm.ErrorMsg.Add("填空题需要包含___");
                                                        checkTitankongti = false;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                htmlAnswer = htmlAnswer.ToUpper();
                                                if (tmTitle_option_remark_list.Count >= 4)
                                                {
                                                    for (var i = 1; i < tmTitle_option_remark_list.Count - 1; i++)
                                                    {
                                                        var option = tmTitle_option_remark_list[i];
                                                        option = StringUtils.StripTags(option);

                                                        options.Add(option);
                                                        answers.Add(StringUtils.GetABC()[i - 1]);
                                                    }
                                                }
                                                else
                                                {
                                                    checkOptions = false;
                                                    errorTotal++;
                                                    errorTm.ErrorMsg.Add("请检查候选项");
                                                }


                                                if (answers.Count > 0)
                                                {
                                                    for (int answerIndex = 0; answerIndex < answers.Count; answerIndex++)
                                                    {
                                                        try
                                                        {
                                                            if (!htmlAnswer.Contains(answers[answerIndex]))
                                                            {
                                                                answers[answerIndex] = string.Empty;
                                                            }
                                                        }
                                                        catch { continue; }
                                                    }
                                                }
                                                if (options.Count > 0 && answers.Count > 0)
                                                {
                                                    var hasAnswer = false;
                                                    foreach (var answer in answers)
                                                    {
                                                        if (!string.IsNullOrEmpty(answer))
                                                        {
                                                            hasAnswer = true;
                                                        }
                                                    }
                                                    if (!hasAnswer || answers.Count > options.Count || (htmlAnswer.Length > 1 && (tx.ExamTxBase == ExamTxBase.Danxuanti || tx.ExamTxBase == ExamTxBase.Panduanti)))
                                                    {
                                                        checkOptions = false;
                                                        errorTotal++;
                                                        errorTm.ErrorMsg.Add("答案和候选项不匹配");
                                                    }
                                                }
                                                else
                                                {
                                                    checkOptions = false;
                                                    errorTotal++;
                                                    errorTm.ErrorMsg.Add("请检查题目候选项");
                                                }

                                            }


                                            var nd = 1;
                                            try
                                            {
                                                nd = TranslateUtils.ToInt(htmlNd);
                                            }
                                            catch { }
                                            decimal score = 0;
                                            try
                                            {
                                                score = TranslateUtils.ToDecimal(htmlScore);
                                            }
                                            catch { }


                                            if (await _examTmRepository.ExistsAsync(tmTitle, tx.Id))
                                            {
                                                errorTotal++;
                                                errorTm.ErrorMsg.Add("题库中已存在相同题型的题目");
                                            }
                                            else
                                            {
                                                if (checkOptions && checkTitankongti)
                                                {

                                                    var info = new ExamTm
                                                    {
                                                        TreeId = treeId,
                                                        TxId = tx.Id,
                                                        Answer = htmlAnswer,
                                                        Title = tmTitle,
                                                        Score = score,
                                                        Nandu = nd,
                                                        Zhishidian = htmlZsd,
                                                        Jiexi = htmlJx,

                                                        CompanyId = adminAuth.CurCompanyId,
                                                        DepartmentId = admin.DepartmentId,
                                                        CreatorId = admin.Id,
                                                        CompanyParentPath = adminAuth.CompanyParentPath,
                                                        DepartmentParentPath = admin.DepartmentParentPath,
                                                    };

                                                    info.Set("options", options);
                                                    info.Set("optionsValues", answers);

                                                    successTotal++;
                                                    successTmList.Add(info);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        errorTotal++;
                                        errorTm.ErrorMsg.Add("题目注释不完整");
                                    }
                                }
                                else
                                {
                                    errorTotal++;
                                    errorTm.ErrorMsg.Add("题目不完整，没有题目内容或者题目注释");
                                }
                            }
                            else
                            {
                                errorTotal++;
                                errorTm.ErrorMsg.Add("没有题目注释");
                            }
                            errorTms.Add(errorTm);
                        }
                    }
                }
            }

            foreach (var error in errorTms)
            {
                if (error.ErrorMsg != null && error.ErrorMsg.Count > 0)
                {
                    error.TmHtml = StringUtils.ReplaceEndsWithIgnoreCase(error.TmHtml, "<p></p>", string.Empty);
                    error.TmHtml = StringUtils.ReplaceEndsWithIgnoreCase(error.TmHtml, "<p><br/></p>", string.Empty);
                    error.TmHtml = error.TmHtml.Replace(error.TmHtml, $"{error.TmHtml}<xblm><p style='color:red;'>{ListUtils.ToString(error.ErrorMsg, "，")}</p></xblm>");
                }

                resultTmHtml += $"{error.TmHtml}<p><br/></p>";
            }

            return (total, successTotal, errorTotal, successTmList, resultTmHtml);

        }
    }
}
