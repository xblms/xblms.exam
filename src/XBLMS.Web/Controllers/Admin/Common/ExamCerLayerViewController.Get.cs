using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class ExamCerLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<ItemResult<ExamCer>>> Get([FromQuery] IdRequest request)
        {
            var cer = await _examCerRepository.GetAsync(request.Id);
            cer.Set("UserCount", await _examCerUserRepository.GetCountAsync(cer.Id));
            return new ItemResult<ExamCer>
            {
                Item = cer
            };
        }



    }
}
