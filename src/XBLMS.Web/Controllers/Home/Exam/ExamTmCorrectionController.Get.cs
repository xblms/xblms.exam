using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamTmCorrectionController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var tm = await _examTmRepository.GetAsync(request.Id);
            if (tm == null) return this.Error("原题已经被删除，请忽略");
            var (total, list) = await _examTmCorrectionRepository.GetListAsync(user.Id, request.Id);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    item.Set("AuditStatusStr", item.AuditStatus.GetDisplayName());
                }
            }

            return new GetResult
            {
                Total = total,
                List = list,
                Title = _examManager.GetTmTitle(tm)
            };
        }
    }
}
