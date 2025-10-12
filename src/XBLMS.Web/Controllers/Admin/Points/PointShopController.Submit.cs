using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Points
{
    public partial class UtilitiesPointShopController
    {
        [HttpPost, Route(RouteItem)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] GetItem request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();
            var admin = adminAuth.Admin;

            var gift = request.Item;
            if (gift.Id > 0)
            {
                await _pointShopRepository.UpdateAsync(gift);
                await _authManager.AddAdminLogAsync("修改商品", gift.Name);
                await _authManager.AddStatLogAsync(StatType.GiftUpdate, "修改商品", gift.Id, gift.Name);
            }
            else
            {
                gift.CompanyId = adminAuth.CurCompanyId;
                gift.DepartmentId = admin.DepartmentId;
                gift.CreatorId = admin.Id;
                gift.CompanyParentPath = adminAuth.CompanyParentPath;
                gift.DepartmentParentPath = admin.DepartmentParentPath;
                await _pointShopRepository.InsertAsync(gift);

                await _authManager.AddAdminLogAsync("新增商品", gift.Name);
                await _authManager.AddStatLogAsync(StatType.GiftAdd, "新增商品", gift.Id, gift.Name);
                await _authManager.AddStatCount(StatType.GiftAdd);
            }


            return new BoolResult
            {
                Value = true
            };
        }
    }
}
