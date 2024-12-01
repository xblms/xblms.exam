using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Core.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPkController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var (total, list) = await _examPkRepository.GetListAsync(request.KeyWords, request.PageIndex, request.PageSize);
            if (total > 0)
            {

                foreach (var item in list)
                {
                    var userIds = await _examPkUserRepository.GetUserIdsAsync(item.Id);
                    var userTotal = 0;
                    var haveMe = false;
                    if(userIds!=null && userIds.Count > 0)
                    {
                        userTotal = userIds.Count;
                        if (userIds.Contains(user.Id))
                        {
                            haveMe = true;
                        }
                    }

                    item.Set("PkBeginDateTimeStr", item.PkBeginDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
                    item.Set("PkEndDateTimeStr", item.PkEndDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
                    item.Set("UserTotal", userTotal);
                    item.Set("HaveMe", haveMe);
                }
            }
            return new GetResult
            {
                List = list,
                Total = total,
            };
        }
    }
}
