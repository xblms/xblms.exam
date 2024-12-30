using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBLMS.Configuration;
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
    public partial class ExamPaperExportLayerController : ControllerBase
    {
        private const string Route = "common/examPaperExportLayer";
        private const string RouteScore = Route + "/score";

        private readonly IExamTmRepository _examTmRepository;
        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IExamManager _examManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperRandomRepository _examPaperRandomRepository;
        private readonly IExamPaperRandomTmRepository _examPaperRandomTmRepository;
        private readonly IExamPaperRandomConfigRepository _examPaperRandomConfigRepository;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IExamPaperAnswerRepository _examPaperAnswerRepository;

        public ExamPaperExportLayerController(IAuthManager authManager, IPathManager pathManager, IExamTmRepository examTmRepository, IExamManager examManager, IExamPaperRepository examPaperRepository, IExamPaperRandomRepository examPaperRandomRepository, IExamPaperRandomTmRepository examPaperRandomTmRepository, IExamPaperRandomConfigRepository examPaperRandomConfigRepository, IExamTxRepository examTxRepository, IExamPaperStartRepository examPaperStartRepository, IUserRepository userRepository, IExamPaperAnswerRepository examPaperAnswerRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _examTmRepository = examTmRepository;
            _examManager = examManager;
            _examPaperRepository = examPaperRepository;
            _examPaperRandomRepository = examPaperRandomRepository;
            _examPaperRandomTmRepository = examPaperRandomTmRepository;
            _examPaperRandomConfigRepository = examPaperRandomConfigRepository;
            _examTxRepository = examTxRepository;
            _examPaperStartRepository = examPaperStartRepository;
            _userRepository = userRepository;
            _examPaperAnswerRepository = examPaperAnswerRepository;
        }


        public class GetReqest
        {
            public int Id { get; set; }
            public int PaperId { get; set; }
            public bool WithAnswer { get; set; }
            public ExamPaperExportType Type { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string Keywords { get; set; }
        }



        public void GetHead(StringBuilder wordContent, List<ExamPaperRandomConfig> configs)
        {
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
            for (var i = 0; i <= configs.Count; i++)
            {
                wordContent.Append($"<td style='padding:8px;text-align:center;border:1px solid #000000;'></td>");
            }
            wordContent.Append("</tr>");
            wordContent.Append("</table>");
        }
        public async Task GetTm(StringBuilder wordContent, List<ExamPaperRandomConfig> configs, int randomId, bool withAnswer, ExamPaperStart start = null, ExamPaper paper = null, bool isScore = false)
        {
            var txIndex = 1;
            var tmIndex = 1;
            var answerList = new List<KeyValuePair<int, string>>();
            foreach (var config in configs)
            {
                var tms = await _examPaperRandomTmRepository.GetListAsync(randomId, config.TxId);
                var txTotalScore = tms.Sum(t => t.Score);
                var txTotalTm = tms.Count;
                wordContent.Append($"<p style='font-weight:bold;'>{StringUtils.ParseNumberToChinese(txIndex)}、{config.TxName}（共{txTotalTm}题，共{txTotalScore}分）</p>");


                if (tms! != null && tms.Count > 0)
                {
                    foreach (var tm in tms)
                    {
                        var answer = new ExamPaperAnswer();
                        if (isScore)
                        {
                            answer = await _examPaperAnswerRepository.GetAsync(tm.Id, start.Id, paper.Id);
                        }

                        await _examManager.GetTmInfoByPaper(tm);
                        var tx = await _examTxRepository.GetAsync(tm.TxId);
                        answerList.Add(new KeyValuePair<int, string>(tmIndex, tm.Answer));

                        var tmTitle = tm.Get("TitleHtml").ToString().Trim();
                        if (StringUtils.StartsWithIgnoreCase(tmTitle, "<p>"))
                        {
                            tmTitle = StringUtils.ReplaceStartsWithIgnoreCase(tmTitle, "<p>", $"<p>{tmIndex}.");
                            if (isScore)
                            {
                                tmTitle = StringUtils.ReplaceEndsWithIgnoreCase(tmTitle, "</p>", $"（{answer.Score}/{tm.Score}分）</p>");
                            }
                            else
                            {
                                tmTitle = StringUtils.ReplaceEndsWithIgnoreCase(tmTitle, "</p>", $"（{tm.Score}分）</p>");
                            }

                            wordContent.Append(tmTitle);
                        }
                        else
                        {
                            if (isScore)
                            {
                                wordContent.Append($"<p>{tmIndex}.{tmTitle}（{answer.Score}/{tm.Score}分）</p>");
                            }
                            else
                            {
                                wordContent.Append($"<p>{tmIndex}.{tmTitle}（{tm.Score}分）</p>");
                            }

                        }


                        if (tx.ExamTxBase != ExamTxBase.Tiankongti && tx.ExamTxBase != ExamTxBase.Jiandati)
                        {
                            var options = ListUtils.ToList(tm.Get("options"));
                            if (options != null)
                            {
                                if (options.Count > 0)
                                {
                                    for (var oi = 0; oi < options.Count; oi++)
                                    {
                                        var option = options[oi];
                                        if (!string.IsNullOrWhiteSpace(option))
                                        {
                                            if (StringUtils.StartsWithIgnoreCase(option, "<p>"))
                                            {
                                                option = StringUtils.ReplaceStartsWithIgnoreCase(option, "<p>", $"<p>{StringUtils.GetABC()[oi]}.");
                                                wordContent.Append(option);
                                            }
                                            else
                                            {
                                                wordContent.Append($"<p>{StringUtils.GetABC()[oi]}.{option}</p>");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (isScore)
                        {
                            wordContent.Append($"<p>考生答案：{answer.Answer}</p>");
                            if(withAnswer)
                            {
                                wordContent.Append($"<p>标准答案：{tm.Answer}</p>");
                            }
                        }
                 
                       
                        tmIndex++;
                        wordContent.Append($"<p></p>");
                    }
                }
                wordContent.Append($"<p></p>");
                txIndex++;
            }

            if (withAnswer)
            {
                wordContent.Append($"<p></p>");
                wordContent.Append($"<p></p>");
                wordContent.Append($"<h2>标准答案</h2>");
                wordContent.Append($"<p>");
                foreach (var answer in answerList)
                {
                    wordContent.Append($"{answer.Key}：{answer.Value}；");
                }
                wordContent.Append($"</p>");
            }
        }
    }
}
