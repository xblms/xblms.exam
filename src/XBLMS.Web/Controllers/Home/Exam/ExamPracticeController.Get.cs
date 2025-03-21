using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetCreateResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var (total, list) = await _examPracticeRepository.GetListAsync(user.Id, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var txNames = new List<string>();
                    if (item.TxIds != null && item.TxIds.Count > 0)
                    {
                        foreach (var txId in item.TxIds)
                        {
                            var tx = await _examTxRepository.GetAsync(txId);
                            if (tx != null)
                            {
                                txNames.Add(tx.Name);
                            }
                        }
                    }
                    item.Set("TxNames", txNames);
                }
            }
            return new GetCreateResult
            {
                List = list,
                Total = total,
            };
        }
        [HttpGet, Route(RouteTotal)]
        public async Task<ActionResult<GetTotalResult>> Get()
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var (collectTotal, wrongTotal) = await _examManager.AnalysisPracticeTmTotalOnlyCollectAndWrong(user.Id);

            return new GetTotalResult
            {
                CollectTotal = collectTotal,
                WrongTotal = wrongTotal
            };
        }
    }
}
