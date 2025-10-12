using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserDocLayerViewController
    {
        [HttpGet, Route(RoutePointShopLog)]
        public async Task<ActionResult<GetPointShopLogResult>> GetPointShopLog([FromQuery] GetRequest request)
        {
            var (total, list) = await _databaseManager.PointShopUserRepository.Analysis_GetListAsync(request.Id, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    item.Set("UserStateStr", item.UserState.GetDisplayName());
                }
            }
            return new GetPointShopLogResult
            {
                Total = total,
                List = list
            };
        }

        public class GetPointShopLogResult
        {
            public int Total { get; set; }
            public List<PointShopUser> List { get; set; }
        }
    }
}
