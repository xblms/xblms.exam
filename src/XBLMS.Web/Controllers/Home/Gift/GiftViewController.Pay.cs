using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Home.Gift
{
    public partial class GiftViewController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Pay([FromBody] GetPayRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var gift = await _pointShopRepository.GetAsync(request.Id);
            var shopUser = new PointShopUser
            {
                ShopId = request.Id,
                ShopName = gift.Name,
                Point = gift.Point,
                UserId = user.Id,
                CompanyId = user.CompanyId,
                DepartmentId = user.DepartmentId,
                CreatorId = user.Id,
                CompanyParentPath = user.CompanyParentPath,
                DepartmentParentPath = user.DepartmentParentPath,
                ShopType = request.ShopType,
                Contacts = request.LinkMan,
                ContactMobile = request.LinkTel,
                ContactAddress = request.LinkAddress,
                KeyWords = gift.Name,
                KeyWordsAdmin = await _organManager.GetUserKeyWords(user.Id) + "-" + gift.Name
            };

            if (shopUser.ShopType == PointShopType.Online)
            {
                shopUser.UserState = PointShopState.Daifahuo;
                shopUser.AdminState = PointShopState.Daifahuo;

                await _userRepository.UpdatePointShopInfoAsync(request.LinkMan, request.LinkTel, request.LinkAddress, user.Id);
            }
            else
            {
                shopUser.UserState = PointShopState.Dailing;
                shopUser.AdminState = PointShopState.Dailing;
            }

            gift.ShopTotal = gift.ShopTotal + 1;
            await _pointShopRepository.UpdateAsync(gift);

            await _pointShopUserRepository.InsertAsync(shopUser);

            await _userRepository.UpdatePointsSurplusAsync(gift.Point, user.Id);

            user = await _userRepository.GetByUserIdAsync(user.Id);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
