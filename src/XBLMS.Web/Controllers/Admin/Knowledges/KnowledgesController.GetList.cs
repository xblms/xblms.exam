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

            var adminAuth = await _authManager.GetAdminAuth();

            var (total, list) = await _knowlegesRepository.GetListAsync(adminAuth, request.TreeId, request.TreeIsChildren, request.Keywords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    item.CoverImgUrl = await _pathManager.GetServerFileUrl(item.CoverImgUrl);
                }
            }
            return new GetResult
            {
                Total = total,
                List = list
            };
        }
    }
}
