using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Home.Gift
{
    public partial class GiftsController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var (total, list) = await _pointShopRepository.GetListAsync(user.CompanyId, request.Point, request.Keywords, request.PageIndex, request.PageSize);
      
            return new GetResult
            {
                Total = total,
                List = list
            };
        }
    }
}
