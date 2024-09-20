using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperMoniController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();

            var paperIds = await _examPaperUserRepository.GetPaperIdsByUser(user.Id, request.Date);
            var (total, list) = await _examPaperRepository.GetListByUserAsync(paperIds, request.KeyWords, request.PageIndex, request.PageSize, true);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    await _examManager.GetPaperInfo(item, user);
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
