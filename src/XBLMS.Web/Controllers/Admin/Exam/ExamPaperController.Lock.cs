using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperController
    {
        [HttpPost, Route(RouteLock)]
        public async Task<ActionResult<BoolResult>> Lock([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var paper = await _examPaperRepository.GetAsync(request.Id);
            if (paper == null) return NotFound();
            paper.Locked = true;

            await _examPaperRepository.UpdateAsync(paper);
            await _examPaperUserRepository.UpdateLockedAsync(paper.Id, paper.Locked);
            await _examPaperStartRepository.UpdateLockedAsync(paper.Id, paper.Locked);

            await _authManager.AddAdminLogAsync("锁定试卷", $"{paper.Title}");
            await _authManager.AddStatLogAsync(StatType.ExamUpdate, "禁用考试", paper.Id, paper.Title);

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
