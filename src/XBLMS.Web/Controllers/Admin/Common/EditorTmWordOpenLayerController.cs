using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
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

        public EditorTmWordOpenLayerController(IHttpContextAccessor context, IAuthManager authManager, IExamTmRepository examTmRepository, IExamTxRepository examTxRepository, IStatRepository statRepository)
        {
            _context = context;
            _authManager = authManager;
            _examTmRepository = examTmRepository;
            _examTxRepository = examTxRepository;
            _statRepository = statRepository;
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


        public async Task<(int total, int successTotal, int errorTotal, List<ExamTm> successTmList, string resultTmHtml)> Check(string tmHtml, int treeId, Administrator admin)
        {
            var total = 0;
            var successTotal = 0;
            var errorTotal = 0;

            var successTmList = new List<ExamTm>();
            var errorTms = new List<CheckErrorMsg>();


            var regFront = @"<xblm>(.*?)</xblm>";
            tmHtml = Regex.Replace(tmHtml, regFront, string.Empty, RegexOptions.IgnoreCase);

            var resultTmHtml = string.Empty;

            if (!string.IsNullOrEmpty(tmHtml))
            {
                var tmhtmlList = ListUtils.GetStringList(tmHtml, "<p><br/></p>");
                ListUtils.RemoveIgnoreCase(tmhtmlList, "");

                if (tmhtmlList != null && tmhtmlList.Count > 0)
                {
                    foreach (var tmhtml in tmhtmlList)
                    {
                        if (!string.IsNullOrEmpty(tmhtml))
                        {
                            total++;

                            var errorTm = new CheckErrorMsg();
                            errorTm.ErrorMsg = new List<string>();
                            errorTm.TmHtml = tmhtml;

                            if (tmhtml.Contains("题目注释"))
                            {
                                var tmContentAndRemarkList = ListUtils.GetStringList(tmhtml, "题目注释");
                                ListUtils.RemoveIgnoreCase(tmContentAndRemarkList, "");

                                if (tmContentAndRemarkList != null && tmContentAndRemarkList.Count == 2)
                                {
                                    var tmContentHtml = tmContentAndRemarkList[0];
                                    var tmRemarkHtml = HtmlUtils.ClearElementAttributes(tmContentAndRemarkList[1], "p");

                                    var tmRemarkList = ListUtils.GetStringList(tmRemarkHtml, "|");
                                    ListUtils.RemoveIgnoreCase(tmRemarkList, "");

                                    if (tmRemarkList != null && tmRemarkList.Count == 6)
                                    {
                                        var htmlTitle = "";
                                        var htmlTx = StringUtils.Trim(tmRemarkList[0]);
                                        var htmlAnswer = tmRemarkList[1].Trim().ToUpper();
                                        var htmlScore = tmRemarkList[2].Trim();
                                        var htmlNd = tmRemarkList[3].Trim();
                                        var htmlZsd = tmRemarkList[4].Trim();
                                        var htmlJx = tmRemarkList[5].Trim();
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
                                            if (tx.ExamTxBase == ExamTxBase.Tiankongti || tx.ExamTxBase == ExamTxBase.Jiandati)
                                            {
                                                htmlTitle = tmContentHtml;
                                            }
                                            else
                                            {
                                                var tmRowHtmlList = ListUtils.GetStringList(tmContentHtml, "</p><p>");
                                                ListUtils.RemoveIgnoreCase(tmRowHtmlList, "");

                                                if (tmRowHtmlList != null && tmRowHtmlList.Count > 1)
                                                {
                                                    htmlTitle = tmRowHtmlList[0];

                                                    for (var i = 1; i < tmRowHtmlList.Count; i++)
                                                    {
                                                        var option = HtmlUtils.ClearElementAttributes(tmRowHtmlList[i], "p");
                                                        options.Add(option);
                                                        answers.Add(StringUtils.GetABC()[i - 1]);
                                                    }
                                                }
                                            }

                                            htmlTitle = HtmlUtils.ClearElementAttributes(htmlTitle, "p");

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

                                            if (answers.Count > 0)
                                            {
                                                for (int answerIndex = 0; answerIndex < answers.Count; answerIndex++)
                                                {
                                                    if (!htmlAnswer.Contains(answers[answerIndex]))
                                                    {
                                                        answers[answerIndex] = "";
                                                    }
                                                }
                                            }

                                            var checkOptions = true;
                                            if (tx.ExamTxBase != ExamTxBase.Tiankongti && tx.ExamTxBase != ExamTxBase.Jiandati)
                                            {
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
                                                    if (!hasAnswer)
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

                                            if (await _examTmRepository.ExistsAsync(htmlTitle, tx.Id))
                                            {
                                                errorTotal++;
                                                errorTm.ErrorMsg.Add("题库中已存在相同题型的题目");
                                            }
                                            else
                                            {
                                                if (checkOptions)
                                                {
                                                    var info = new ExamTm();
                                                    info.TreeId = treeId;
                                                    info.TxId = tx.Id;
                                                    info.Answer = htmlAnswer;
                                                    info.Title = htmlTitle;
                                                    info.Score = score;
                                                    info.Nandu = nd;
                                                    info.Zhishidian = htmlZsd;
                                                    info.Jiexi = htmlJx;

                                                    info.CompanyId = admin.CompanyId;
                                                    info.DepartmentId = admin.DepartmentId;
                                                    info.CreatorId = admin.Id;

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
                                        errorTm.ErrorMsg.Add("请检查题目注释是否有多余的内容");
                                    }
                                }
                                else
                                {
                                    errorTotal++;
                                    errorTm.ErrorMsg.Add("请检查题目内部是否有多余的换行");
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
                var tmhtml = StringUtils.ReplaceEndsWithIgnoreCase(error.TmHtml, "<p><br/></p>", string.Empty);
                tmhtml = StringUtils.ReplaceEndsWithIgnoreCase(error.TmHtml, "<p></p>", string.Empty);

                if (error.ErrorMsg != null && error.ErrorMsg.Count > 0)
                {
                    tmhtml = tmhtml.Replace(tmhtml, $"{tmhtml}<xblm><p style='color:red;'>{ListUtils.ToString(error.ErrorMsg, "，")}</p></xblm>");
                }
                resultTmHtml += $"{tmhtml}<p><br/></p>";
            }


            return (total, successTotal, errorTotal, successTmList, resultTmHtml);

        }
    }
}
