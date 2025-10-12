using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseEvaluationLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var item = await _studyCourseEvaluationRepository.GetAsync(request.Id);
            var itemList = await _studyCourseEvaluationItemRepository.GetListAsync(request.Id);
            if (itemList != null && itemList.Count > 0) {
                foreach (var itemInfo in itemList)
                {
                    itemInfo.Set("Score", item.MaxStar);
                }
            }
            return new GetResult
            {
                Item = item,
                ItemList = itemList,
            };
        }



    }
}
