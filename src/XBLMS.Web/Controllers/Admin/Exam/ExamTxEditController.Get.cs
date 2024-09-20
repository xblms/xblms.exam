using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            if (request.Id > 0)
            {
                tx = await _examTxRepository.GetAsync(request.Id);
            }
            return new GetResult
            {
                Item = tx,
                TypeList = typeList
            };
        }
    }
}
