using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmSelectController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteRemove)]
        public async Task<ActionResult<BoolResult>> Remove([FromBody] GetSeletRemoveRequest request)
        {
            var group = await _examTmGroupRepository.GetAsync(request.Id);
            if (request.Ids != null && request.Ids.Count > 0)
            {
                var minusTotal = 0;
                foreach (var tmId in request.Ids)
                {
                    var tm = await _examTmRepository.GetAsync(tmId);
                    if (tm.TmGroupIds != null)
                    {
                        if (tm.TmGroupIds.Contains($"'{group.Id}'"))
                        {
                            minusTotal++;
                            tm.TmGroupIds.Remove($"'{group.Id}'");
                            await _examTmRepository.UpdateTmGroupIdsAsync(tm);
                        }
                    }
                }
                group.TmTotal = group.TmTotal - minusTotal;
                await _examTmGroupRepository.UpdateAsync(group);
            }
            await _authManager.AddAdminLogAsync("题目组移出题目", $"{group.GroupName}");
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
