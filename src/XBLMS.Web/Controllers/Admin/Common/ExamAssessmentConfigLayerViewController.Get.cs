using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class ExamAssessmentConfigLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var item = await _examAssessmentConfigRepository.GetAsync(request.Id);
            var itemList = await _examAssessmentConfigSetRepository.GetListAsync(request.Id);

            return new GetResult
            {
                Item = item,
                ItemList = itemList,
            };
        }



    }
}
