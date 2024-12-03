using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamAssessmentConfigEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var item = new ExamAssessmentConfig();
            item.Title = "测评类别-" + StringUtils.PadZeroes(await _examAssessmentConfigRepository.MaxAsync(), 5);

            var itemList = new List<ExamAssessmentConfigSet>();

            if (request.Id > 0)
            {
                item = await _examAssessmentConfigRepository.GetAsync(request.Id);

                var items = await _examAssessmentConfigSetRepository.GetListAsync(item.Id);
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
