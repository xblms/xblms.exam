using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTxController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest reqeust)
        {
            var list = await _examTxRepository.GetListAsync();
            if (list == null || list.Count == 0)
            {
                await _examTxRepository.ResetAsync();
                list = await _examTxRepository.GetListAsync();
            }

            var resultList = new List<ExamTx>();
           
            foreach (var tx in list)
            {
                tx.Set("TmCount", await _examTmRepository.GetCountByTxIdAsync(tx.Id));
                tx.Set("TxBaseName", tx.ExamTxBase.GetDisplayName());

                if (!string.IsNullOrEmpty(reqeust.Name))
                {
                    if (tx.Name.Contains(reqeust.Name))
                    {
                        resultList.Add(tx);
                    }
                }
                else
                {
                    resultList.Add(tx);
                }
            }
            resultList = resultList.OrderBy(tx => tx.Taxis).ToList();
            return new GetResult
            {
                Items = resultList
            };
        }
    }
}
