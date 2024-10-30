using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using Ubiety.Dns.Core;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetList([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var start = await _examPaperStartRepository.GetAsync(request.Id);

            var paper = await _examPaperRepository.GetAsync(start.ExamPaperId);

            if (paper == null || start == null) { return NotFound(); }

            var randomId = start.ExamPaperRandomId;
            var startId = request.Id;

            if (randomId == 0) { return this.Error("试卷发布有问题，请联系管理员：找不到随机策略！"); }

            var configs = await _examPaperRandomConfigRepository.GetListAsync(paper.Id);
            if (configs == null || configs.Count == 0) { return this.Error("试卷发布有问题：找不到任何题目！"); }

            var paperTmTotal = 0;

            var tmIndex = 1;
            foreach (var config in configs)
            {
                var tms = await _examPaperRandomTmRepository.GetListAsync(randomId, config.TxId);
                if (tms != null && tms.Count > 0)
                {
                    paperTmTotal += tms.Count;

                    foreach (var item in tms)
                    {
                        await _examManager.GetTmInfoByPaperUser(item, paper, startId, true);
                        item.Set("TmIndex", tmIndex);
                        tmIndex++;
                    }
                    config.Set("TmList", tms);
                }
            }

            paper.Set("TmTotal", paperTmTotal);
            paper.Set("StartId", startId);

            paper.Set("UserDisplayName", user.DisplayName);
            paper.Set("UserAvatar", user.AvatarUrl);

            paper.Set("UseTime", DateUtils.SecondToHms(start.ExamTimeSeconds));
            paper.Set("UserScore", start.Score);

            return new GetResult
            {
                Watermark = await _authManager.GetWatermark(),
                Item = paper,
                TxList = configs
            };
        }
    }
}
