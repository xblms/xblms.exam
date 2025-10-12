using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Points
{
    public partial class UtilitiesPointShopController
    {
        [HttpGet, Route(RouteUsers)]
        public async Task<ActionResult<GetUsersResult>> GetUserList([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();

            var (total, list) = await _pointShopUserRepository.GetListAsync(adminAuth, request.Id, request.KeyWords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var gift = await _pointShopRepository.GetAsync(item.ShopId);
                    var user = await _userRepository.GetByUserIdAsync(item.UserId);
                    item.Set("User", user);
                    item.Set("Gift", gift);
                    item.Set("AdminStateStr", item.AdminState.GetDisplayName());
                    item.Set("ShopTypeStr", item.ShopType.GetDisplayName());
                }
            }

            return new GetUsersResult
            {
                Total = total,
                List = list
            };
        }
        [HttpPost, Route(RouteUsers)]
        public async Task<ActionResult<BoolResult>> SetState([FromBody] GetSetStateRequest request)
        {
            var shopUser = await _pointShopUserRepository.GetAsync(request.Id);
            shopUser.AdminState = request.State;
            shopUser.UserState = request.State;

            await _pointShopUserRepository.UpdateAsync(shopUser);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
