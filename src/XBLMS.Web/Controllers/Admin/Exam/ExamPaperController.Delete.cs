using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using XBLMS.Dto;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {

            var paper = await _examPaperRepository.GetAsync(request.Id);
            if (paper != null)
            {
                await _examPaperRepository.DeleteAsync(paper.Id);
                await _examManager.ClearRandom(paper.Id, true);
                await _authManager.AddAdminLogAsync("删除试卷", $"试卷名称：{paper.Title}");

            }
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
