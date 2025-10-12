using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamCerController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest reqeust)
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var list = await _examCerRepository.GetListAsync(adminAuth, reqeust.Title);

            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.Set("PaperCount", await _examPaperRepository.GetCerCount(item.Id));
                    item.Set("UserCount", await _examCerUserRepository.GetCountAsync(item.Id));
                }
            }
            return new GetResult
            {
                Items = list,
            };
        }
    }
}
