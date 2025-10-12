using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseTreeController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var auth = await _authManager.GetAdminAuth();

            var trees = await _studyManager.GetStudyCourseTreeCascadesAsync(auth);

            return new GetResult
            {
                Items = trees,
            };
        }
    }
}
