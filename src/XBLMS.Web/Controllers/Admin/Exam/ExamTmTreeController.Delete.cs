using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmTreeController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();

            var item = await _examTmTreeRepository.GetAsync(request.Id);

            if (item == null) return this.NotFound();
            var ids = await _examTmTreeRepository.GetIdsAsync(request.Id);
            var tmCount =0;
            if (tmCount > 0) return this.Error($"该分类下面包含【{tmCount}】题目，暂时不允许删除");
            await _examTmTreeRepository.DeleteAsync(ids);
            await _authManager.AddAdminLogAsync("删除题目分类及所有下级", $"{item.Name}");
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
