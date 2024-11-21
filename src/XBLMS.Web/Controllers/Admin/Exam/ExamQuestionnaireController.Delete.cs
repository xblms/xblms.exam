using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var paper = await _examQuestionnaireRepository.GetAsync(request.Id);
            if (paper != null)
            {
                await _examQuestionnaireRepository.DeleteAsync(paper.Id);
                await _examManager.ClearQuestionnaire(paper.Id);
                await _authManager.AddAdminLogAsync("删除问卷调查", $"{paper.Title}");

            }
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
