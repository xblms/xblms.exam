using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireQrcodeController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var paper = await _examQuestionnaireRepository.GetAsync(request.Id);
            var host = Request.Host.Port.HasValue ? $"{Request.Host.Host}:{Request.Host.Port}" : Request.Host.Host;
            var hostUrl = Request.Scheme + "://" + PageUtils.Combine(host, $"home/exam/examQuestionnairing?pr={StringUtils.Guid()}&ps={paper.Guid}");

            return new GetResult
            {
                Title = paper.Title,
                QrcodeUrl = hostUrl,
            };
        }
    }
}
