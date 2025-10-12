using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Points
{
    public partial class UtilitiesPointShopController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetList([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();

            var (total, list) = await _pointShopRepository.GetListAsync(adminAuth, request.KeyWords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var creator = await _administratorRepository.GetByUserIdAsync(item.CreatorId);
                    if (creator != null)
                    {
                        item.Set("Creator", creator.DisplayName);
                        item.Set("CreatorId", creator.Id);
                    }
                }
            }

            return new GetResult
            {
                Total = total,
                List = list
            };
        }

        [HttpGet, Route(RouteItem)]
        public async Task<ActionResult<GetItem>> Get([FromQuery] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var fileList = new List<GetUploadResult>();
            var item = new PointShop() { };
            if (request.Id > 0)
            {
                item = await _pointShopRepository.GetAsync(request.Id);
                if (item.CoverImg != null && item.CoverImg.Count > 0)
                {
                    foreach (var img in item.CoverImg)
                    {
                        fileList.Add(new GetUploadResult
                        {
                            Url = img,
                            Name = PathUtils.GetFileNameWithoutExtension(img)
                        });
                    }
                }
            }

            return new GetItem
            {
                FileList = fileList,
                Item = item,
            };
        }
    }
}
