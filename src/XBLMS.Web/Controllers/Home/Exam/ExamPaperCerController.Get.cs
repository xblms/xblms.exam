using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperCerController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();

            var (total, list) = await _examCerUserRepository.GetListAsync(user.Id, 1, int.MaxValue);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var cerInfo = await _examCerRepository.GetAsync(item.CerId);
                    if (cerInfo != null)
                    {
                        item.Set("CerName", cerInfo.Name);
                    }
                    else
                    {
                        item.Set("CerName", "证书异常");
                    }
                    item.Set("AwartDate", item.CerDateTime.Value.ToString(DateUtils.FormatStringDateOnlyCN));
                    var paper = await _examPaperRepository.GetAsync(item.ExamPaperId);
                    if (paper != null)
                    {
                        item.Set("PaperName", paper.Title);
                    }
                    else
                    {
                        item.Set("PaperName", "试卷异常");
                    }
                    var start = await _examPaperStartRepository.GetAsync(item.ExamStartId);
                    if (start != null)
                    {
                        item.Set("PaperScore", start.Score);
                    }
                    else
                    {
                        item.Set("PaperScore", "成绩异常");
                    }
                }
                if (!string.IsNullOrWhiteSpace(request.KeyWords))
                {
                    list = list.Where(cer => {
                        return
                        cer.Get("PaperName").ToString().Contains(request.KeyWords) ||
                        cer.CerNumber.Contains(request.KeyWords) ||
                        cer.Get("CerName").ToString().Contains(request.KeyWords)
                        ;
                    }).ToList();
                }
                if (!string.IsNullOrWhiteSpace(request.DateFrom))
                {
                    list = list.Where(cer => cer.CerDateTime.Value >= TranslateUtils.ToDateTime(request.DateFrom)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(request.DateTo))
                {
                    list = list.Where(cer => cer.CerDateTime.Value <= TranslateUtils.ToDateTime(request.DateTo)).ToList();
                }
            }
            return new GetResult
            {
                List = list
            };
        }
    }
}
