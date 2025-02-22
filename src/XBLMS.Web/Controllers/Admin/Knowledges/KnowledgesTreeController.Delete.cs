using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    public partial class KnowledgesTreeController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var item = await _knowlegesTreeRepository.GetAsync(request.Id);

            if (item == null) return this.NotFound();

            await _knowlegesTreeRepository.DeleteAsync(request.Id);
            await _authManager.AddAdminLogAsync("删除知识库分类及所有下级", $"{item.Name}");
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
