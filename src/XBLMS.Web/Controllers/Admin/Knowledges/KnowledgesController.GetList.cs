using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    public partial class KnowledgesController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetList([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var (total, list) = await _knowlegesRepository.GetListAsync(request.TreeId, request.TreeIsChildren, request.Keywords, request.PageIndex, request.PageSize);

            return new GetResult
            {
                Total = total,
                List = list
            };
        }
    }
}
