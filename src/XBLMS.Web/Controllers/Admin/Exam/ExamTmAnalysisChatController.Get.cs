using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmAnalysisChatController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest reqeust)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var txChats = await _examTmAnalysisTmRepository.GetChatByTxList(reqeust.Id);
            var ndChats = await _examTmAnalysisTmRepository.GetChatByNdList(reqeust.Id);
            var zsdChats = await _examTmAnalysisTmRepository.GetChatByZsdList(reqeust.Id);

            var txs = new List<string>();
            var txvs = new List<int>();
            if (txChats.Count > 0)
            {
                foreach (var txchat in txChats)
                {
                    var tx = await _examTxRepository.GetAsync(txchat.Key);
                    txs.Add(tx.Name);
                    txvs.Add(txchat.Value);
                }
            }

            var nds = new List<string>();
            var ndvs = new List<int>();
            if (ndChats.Count > 0)
            {
                foreach (var ndchat in ndChats)
                {
                    nds.Add($"{ndchat.Key}ÐÇ");
                    ndvs.Add(ndchat.Value);
                }
            }

            var zsds = new List<string>();
            var zsdvs = new List<int>();
            if (zsdChats.Count > 0)
            {
                zsdChats = zsdChats.OrderByDescending(zorder => zorder.Value).ToList();
                foreach (var zsdchat in zsdChats)
                {
                    zsds.Add($"{zsdchat.Key}");
                    zsdvs.Add(zsdchat.Value);

                    if (zsds.Count > 100) break;
                }
            }

            return new GetResult
            {
                TxLabels = txs,
                TxValues = txvs,
                NdLabels = nds,
                NdValues = ndvs,
                ZsdLabels = zsds,
                ZsdValues = zsdvs
            };
        }
    }
}
