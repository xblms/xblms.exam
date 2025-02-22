using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    public partial class KnowledgesTreeController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var trees = await _examManager.GetKnowlegesTreeCascadesAsync(true);

            return new GetResult
            {
                Items = trees,
            };
        }
    }
}
