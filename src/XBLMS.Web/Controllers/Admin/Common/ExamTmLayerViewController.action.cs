using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class ExamTmLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<ItemResult<ExamTm>>> Get([FromQuery] IdRequest request)
        {
            var tm = await _examTmRepository.GetAsync(request.Id);
            await _examManager.GetTmInfo(tm);
            return new ItemResult<ExamTm>
            {
                Item = tm
            };
        }



    }
}
