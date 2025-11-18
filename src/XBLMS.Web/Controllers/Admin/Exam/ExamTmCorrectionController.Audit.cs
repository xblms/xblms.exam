using Datory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmCorrectionController
    {
        [HttpGet, Route(RouteAudit)]
        public async Task<ActionResult<GetAuditResult>> GetAudit([FromQuery] IdRequest request)
        {
            var info = await _examTmCorrectionRepository.GetAsync(request.Id);
            var statusList = ListUtils.GetSelects<ExamTmCorrectionAuditType>();
            if (info.AuditStatus == ExamTmCorrectionAuditType.Info)
            {
                info.AuditStatus = ExamTmCorrectionAuditType.Success;
            }
            return new GetAuditResult
            {
                StatusList = statusList.Where(q => q.Value != ExamTmCorrectionAuditType.Info.GetValue()).ToList(),
               
                Item = new GetAuditItemResult
                {
                    Id = info.Id,
                    AuditResaon = info.AuditResaon,
                    AuditStatus = info.AuditStatus
                }
            };
        }
        [HttpPost, Route(RouteAudit)]
        public async Task<ActionResult<BoolResult>> SubmitAudit([FromBody] GetAuditItemResult request)
        {
            var auth = await _authManager.GetAdminAuth();
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }
            var info = await _examTmCorrectionRepository.GetAsync(request.Id);
            info.AuditStatus = request.AuditStatus;
            info.AuditResaon = request.AuditResaon;
            info.AuditAdminId = auth.AdminId;
            info.AuditDate = DateTime.Now;
            await _examTmCorrectionRepository.UpdateAsync(info);
            await _authManager.AddAdminLogAsync("题目纠错审核", $"{info.TmTitle}");
            await _authManager.AddStatLogAsync(StatType.ExamTmCorrectionAudit, "题目纠错审核", info.Id, info.TmTitle);
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
