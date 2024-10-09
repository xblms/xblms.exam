using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Core.Utils;
using XBLMS.Utils;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperTreeController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();

            var item = await _examPaperTreeRepository.GetAsync(request.Id);

            if (item == null) return this.NotFound();
            var ids = await _examPaperTreeRepository.GetIdsAsync(request.Id);
            var tmCount =0;
            if (tmCount > 0) return this.Error($"该分类下面包含【{tmCount}】份试卷，暂时不允许删除");
            await _examPaperTreeRepository.DeleteAsync(ids);
            await _authManager.AddAdminLogAsync("删除试卷分类及所有下级", $"分类名称：{item.Name}");
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
