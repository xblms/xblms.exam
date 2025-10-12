using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    public partial class KnowledgesEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(Enums.MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var item = await _knowlegesRepository.GetAsync(request.Id);
            return new GetResult
            {
                Item = new GetItemInfo
                {
                    Id=item.Id,
                    Name = item.Name,
                    CoverImgUrl = item.CoverImgUrl,
                    OnlyCompany = item.OnlyCompany,
                },
            };
        }
    }
}
