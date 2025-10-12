using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Gift
{
    public partial class GiftViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var gift = await _pointShopRepository.GetAsync(request.Id);
            gift.Set("MyPoint", user.PointsSurplusTotal);
            gift.Set("LinkMan", user.PointShopLinkMan);
            gift.Set("LinkTel", user.PointShopLinkTel);
            gift.Set("LinkAddress", user.PointShopLinkAddress);
            gift.Set("ShopTypeStr", gift.ShopType.GetDisplayName());

            return new GetResult
            {
                Item = gift,
            };
        }
    }
}
