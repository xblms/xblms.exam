using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class ExamTmLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var tm = await _examTmRepository.GetAsync(request.Id);
            await _examManager.GetTmInfo(tm);

            var tmStyles = await _tableStyleRepository.GetExamTmStylesAsync(false);

            return new GetResult
            {
                Styles = tmStyles,
                Item = tm
            };
        }



    }
}
