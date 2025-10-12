using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class ExamPaperLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var paper = await _examPaperRepository.GetAsync(request.Id);

            if (paper == null) { return NotFound(); }
            var randomIds = await _examPaperRandomRepository.GetIdsByPaperAsync(paper.Id);
            if (randomIds == null || randomIds.Count == 0) { return NotFound(); }
            var configs = await _examPaperRandomConfigRepository.GetListAsync(paper.Id);
            if (configs == null || configs.Count == 0) { return NotFound(); }

            var randomId = request.RandomId;
            if (randomId == 0)
            {
                randomId = randomIds[0];
            }

            var paperTmTotal = 0;
            var tmIndex = 1;
            var txIndex = 1;
            foreach (var config in configs)
            {
                var tmTotal = 0;
                decimal scoreTotal = 0;
                var tms = await _examPaperRandomTmRepository.GetListAsync(randomId, config.TxId, paper.Id);
                if (tms! != null && tms.Count > 0)
                {
                    foreach (var tm in tms)
                    {
                        paperTmTotal++;
                        tmTotal++;
                        scoreTotal += tm.Score;
                        await _examManager.GetTmInfoByPaperAdmin(tm, paper, 0);
                        tm.Set("TmIndex", tmIndex);
                        tmIndex++;
                    }
                }
                config.ScoreTotal = Math.Round(scoreTotal, 2);
                config.TmTotal = tmTotal;
                
                config.Set("TmList", tms);
                config.Set("TxIndex", StringUtils.ParseNumberToChinese(txIndex));
                txIndex++;
            }

            paper.Set("TmTotal", paperTmTotal);
            return new GetResult
            {
                Item = paper,
                RandomIds = randomIds,
                TxList = configs,
                RandomId = randomId
            };
        }
    }
}
