using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Home.Gift
{
    public partial class PointShopLogController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var (total, list) = await _pointShopUserRepository.GetListAsync(user.Id, request.DateFrom, request.DateTo, request.Keywords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    item.Set("UserStateStr", item.UserState.GetDisplayName());
                }
            }

            return new GetResult
            {
                Total = total,
                List = list
            };
        }
    }
}
