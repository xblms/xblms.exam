using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperEditController
    {
        [HttpPost, Route(RouteGetTmList)]
        public async Task<ActionResult<GetTmListResult>> GetTmList([FromBody] GetConfigRequest request)
        {
            var tmList = new List<ExamTm>();
            if (request.TmGroupIds != null && request.TmGroupIds.Count > 0)
            {
                foreach (var tmGroupId in request.TmGroupIds)
                {
                    var group = await _examTmGroupRepository.GetAsync(tmGroupId);
                    if (group != null && group.GroupType == TmGroupType.Fixed)
                    {
                        var getTmList = await _examTmRepository.Group_GetTmListAsync(group);
                        if (getTmList != null)
                        {
                            tmList.AddRange(getTmList);
                        }
                    }
                }
            }
            if (tmList.Count > 0)
            {
                foreach (var tm in tmList)
                {
                    await _examManager.GetTmInfo(tm);
                }
                tmList = tmList.OrderBy(tm=>tm.Get("TxTaxis")).DistinctBy(tm => tm.Id).ToList();
            }
            return new GetTmListResult
            {
                Items = tmList
            };
        }

    }
}
