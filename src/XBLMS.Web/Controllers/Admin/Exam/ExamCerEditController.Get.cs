using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamCerEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var cer = new ExamCer();
            if (request.Id > 0)
            {
                cer = await _examCerRepository.GetAsync(request.Id);
            }
            var families = FontUtils.GetFontFamilies();
            return new GetResult
            {
                Item = cer,
                Fonts = families
            };
        }
    }
}
