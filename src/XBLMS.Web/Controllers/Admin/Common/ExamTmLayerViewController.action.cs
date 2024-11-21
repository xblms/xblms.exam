using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

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
