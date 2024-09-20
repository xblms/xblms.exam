using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    public partial class QueryController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<SubmitResult>> Submit([FromBody] SubmitRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }
            var admin = await _authManager.GetAdminAsync();

            var (isBlocked, _) = await _blockManager.IsBlockedAsync(request.IpAddress, string.Empty);

            var geoNameId = _blockManager.GetGeoNameId(request.IpAddress);
            var area = _blockManager.GetArea(geoNameId);

            return new SubmitResult
            {
                IsAllowed = !isBlocked,
                Area = area
            };
        }
    }
}
