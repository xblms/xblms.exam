using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Points
{
    public partial class UtilitiesPointShopController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var gift = await _pointShopRepository.GetAsync(request.Id);

            if (gift.ShopTotal > 0) return this.Error($"有【{gift.ShopTotal}】兑换记录，暂时不允许删除");
            await _pointShopRepository.DeleteAsync(request.Id);

            await _authManager.AddAdminLogAsync("删除商品", $"{gift.Name}");
            await _authManager.AddStatLogAsync(StatType.GiftDelete, "删除商品", gift.Id, gift.Name, gift);
            await _authManager.AddStatCount(StatType.GiftDelete);
            return new BoolResult
            {
                Value = true
            };

        }
    }
}
