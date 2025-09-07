using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTxEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var typeList = ListUtils.GetSelects<ExamTxBase>();
            var tx = new ExamTx();
            var tmTotal = 0;
            if (request.Id > 0)
            {
                tx = await _examTxRepository.GetAsync(request.Id);
                tmTotal = await _examTmRepository.GetCountByTxIdAsync(tx.Id);
            }
            return new GetResult
            {
                Item = tx,
                TypeList = typeList,
                TmTotal = tmTotal
            };
        }
    }
}
