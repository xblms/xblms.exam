using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class ExamPaperUserMarkLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetList([FromQuery] IdRequest request)
        {
            var start = await _examPaperStartRepository.GetAsync(request.Id);

            var marker = await _administratorRepository.GetByUserIdAsync(start.MarkTeacherId);
   

            var paper = await _examPaperRepository.GetAsync(start.ExamPaperId);

            if (paper == null || start == null) { return NotFound(); }

            var randomId = start.ExamPaperRandomId;
            var startId = request.Id;

            if (randomId == 0) { return this.Error("试卷发布有问题！"); }

            var configs = await _examPaperRandomConfigRepository.GetListAsync(paper.Id);
            if (configs == null || configs.Count == 0) { return this.Error("试卷发布有问题：找不到任何题目！"); }

            var paperTmTotal = 0;

            var tmIndex = 1;
            var txIndex = 1;
            var txList=new List<ExamPaperRandomConfig>();
            foreach (var config in configs)
            {
                var tx = await _examTxRepository.GetAsync(config.TxId);
                if(tx!=null && tx.ExamTxBase==ExamTxBase.Tiankongti || tx.ExamTxBase == ExamTxBase.Jiandati)
                {
                    var tms = await _examPaperRandomTmRepository.GetListAsync(randomId, config.TxId, config.ExamPaperId);
                    if (tms != null && tms.Count > 0)
                    {
                        paperTmTotal += tms.Count;

                        foreach (var item in tms)
                        {
                            await _examManager.GetTmInfoByPaperMark(item, paper, startId);
                            item.Set("TmIndex", tmIndex);
                            tmIndex++;
                        }
                        config.Set("TmList", tms);
                    }
                    config.Set("TxIndex", StringUtils.ParseNumberToChinese(txIndex));
                    txList.Add(config);
                    txIndex++;
                }
            }

            paper.Set("TmTotal", paperTmTotal);
            paper.Set("StartId", startId);

            paper.Set("UserDisplayName", "***");
            paper.Set("UserAvatar", null);

            paper.Set("UseTime", DateUtils.SecondToHms(start.ExamTimeSeconds));

            paper.Set("UserSScore", start.SubjectiveScore);

            var markText = $"阅卷老师账号异常-{start.MarkDateTime}";
            var markName = "/";
            var markDateTime = "/";

            if (marker != null)
            {
                markText = $"{marker.DisplayName}-{marker.UserName}-{paper.Title}-{start.MarkDateTime}";
                markName = $"{marker.DisplayName}（{marker.UserName}）";
                markDateTime = start.MarkDateTime.ToString();
            }

            paper.Set("Marker", markName);
            paper.Set("MarkDateTime", markDateTime);

            return new GetResult
            {
                Watermark = markText,
                Item = paper,
                TxList = txList
            };
        }
    }
}
