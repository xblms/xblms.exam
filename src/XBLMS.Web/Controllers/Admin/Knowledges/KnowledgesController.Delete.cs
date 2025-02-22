using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    public partial class KnowledgesController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (_settingsManager.IsSafeMode)
            {
                return this.Error(Constants.ErrorSafe);
            }

            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var info = await _knowlegesRepository.GetAsync(request.Id);

            await _knowlegesRepository.DeleteAsync(info.Id);

            await _authManager.AddAdminLogAsync("删除知识库", $"{info.Name}");
            

            return new BoolResult
            {
                Value =true
            };
        }
    }
}
