using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamQuestionnaireController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();

            var paperIds = await _examQuestionnaireUserRepository.GetPaperIdsByUser(user.Id);
            var (total, list) = await _examQuestionnaireRepository.GetListByUserAsync(paperIds, request.KeyWords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    await _examManager.GetQuestionnaireInfo(item, user);
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
