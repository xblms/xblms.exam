using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    public partial class KnowledgesTreeController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var trees = await _examManager.GetKnowlegesTreeCascadesAsync(adminAuth, true);

            return new GetResult
            {
                Items = trees,
            };
        }
    }
}
