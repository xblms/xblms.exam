using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseEvaluationEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var item = new StudyCourseEvaluation();
            item.Title = "课程评价-" + StringUtils.PadZeroes(await _studyCourseEvaluationRepository.MaxAsync(), 5);

            var itemList = new List<StudyCourseEvaluationItem>();

            if (request.Id > 0)
            {
                item = await _studyCourseEvaluationRepository.GetAsync(request.Id);

                var items = await _studyCourseEvaluationItemRepository.GetListAsync(item.Id);
                if (items != null && items.Count > 0)
                {
                    foreach (var iteminfo in items)
                    {
                        itemList.Add(iteminfo);
                    }
                }
            }
            return new GetResult
            {
                Item = item,
                ItemList = itemList
            };

        }

    }
}
