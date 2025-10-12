using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyPlanController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetManage([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var auth = await _authManager.GetAdminAuth();

            var (total, list) = await _studyPlanRepository.GetListAsync(auth, request.Keyword, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    item.Set("UserTotal", await _studyPlanUserRepository.GetCountAsync(item.Id, ""));
                    item.Set("PlanDayCount", DateUtils.DateDiff("day", item.PlanBeginDateTime.Value, item.PlanEndDateTime.Value));
                    item.Set("PlanBeginDateTimeStr", item.PlanBeginDateTime.Value.ToString(DateUtils.FormatStringDateOnlyCN));
                    item.Set("PlanEndDateTimeStr", item.PlanEndDateTime.Value.ToString(DateUtils.FormatStringDateOnlyCN));
                }
            }
            return new GetResult
            {
                Total = total,
                List = list,
            };
        }

    }
}
