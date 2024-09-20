using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

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
