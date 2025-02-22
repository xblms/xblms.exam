using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    public partial class KnowledgesController
    {
        [HttpPost, Route(RouteLock)]
        public async Task<ActionResult<BoolResult>> Lock([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var info = await _knowlegesRepository.GetAsync(request.Id);
            info.Locked = true;
            await _knowlegesRepository.UpdateAsync(info);

            await _authManager.AddAdminLogAsync("下架知识库", $"{info.Name}");
            await _authManager.AddStatLogAsync(StatType.KnowledgesUpdate, "下架知识库", info.Id, info.Name);

            return new BoolResult
            {
                Value =true
            };
        }
    }
}
