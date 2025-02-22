using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    public partial class KnowledgesTreeController
    {
        [HttpPost, Route(RouteUpdate)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] ItemRequest<KnowledgesTree> request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var item = await _knowlegesTreeRepository.GetAsync(request.Item.Id);
            item.Name = request.Item.Name;
            await _knowlegesTreeRepository.UpdateAsync(item);
            await _authManager.AddAdminLogAsync("修改知识库分类", $"{item.Name}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
