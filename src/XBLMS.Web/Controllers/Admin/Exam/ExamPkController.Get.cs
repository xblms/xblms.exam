using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPkController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetManage([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var (total, list) = await _examPkRepository.GetListAsync(request.Keyword, request.PageIndex, request.PageSize);

            if (total > 0)
            {
                foreach (var item in list)
                {
                    item.Set("UserTotal", await _examPkUserRepository.CountAsync(item.Id));
                    item.Set("PkBeginDateTimeStr", item.PkBeginDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
                    item.Set("PkEndDateTimeStr", item.PkEndDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
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
