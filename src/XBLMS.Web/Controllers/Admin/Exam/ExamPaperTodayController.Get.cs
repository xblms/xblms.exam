using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperTodayController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();

            var (total, list) = await _examPaperRepository.GetListByDateAsync(adminAuth, request.DateType, false);

            return new GetResult
            {
                Items = list
            };
        }

    }
}
