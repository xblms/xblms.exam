using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmCorrectionController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var auth = await _authManager.GetAdminAuth();
            var statusList = ListUtils.GetSelects<ExamTmCorrectionAuditType>();
            var (total, list) = await _examTmCorrectionRepository.GetListAsync(auth, request.Status, request.KeyWords, request.PageIndex, request.PageSize);

            if (total > 0)
            {
                foreach (var item in list)
                {
                    var user = await _userRepository.GetByUserIdAsync(item.UserId);
                    item.Set("UserDisplayName", user?.DisplayName);
                    if (item.AuditAdminId > 0)
                    {
                        var admin = await _administratorRepository.GetByUserIdAsync(item.AuditAdminId);
                        item.Set("AdminDisplayName", admin?.DisplayName);
                    }
                    item.Set("AuditStatusStr", item.AuditStatus.GetDisplayName());
                }
            }

            return new GetResult
            {
                List = list,
                Total = total,
                StatusList = statusList
            };
        }
    }
}
