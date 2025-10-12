using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class IndexController
    {
        [Authorize(Roles = Types.Roles.Administrator)]
        [HttpPost, Route(RouteChangeAuthDataShowAll)]
        public async Task<ActionResult<BoolResult>> ChangeAuthDataShowAll([FromBody] GetChangeAuthDataShowAllRequest request)
        {
            var admin = await _authManager.GetAdminAsync();

            admin.AuthDataShowAll = request.AuthDataShowAll;

            await _administratorRepository.UpdateAuthDataShowAllAsync(admin);

            return new BoolResult
            {
                Value = true
            };
        }


        [Authorize(Roles = Types.Roles.Administrator)]
        [HttpPost, Route(RouteChangeOrgan)]
        public async Task<ActionResult<BoolResult>> ChangeOrgan([FromBody] ChangeOrganRequest request)
        {
            var admin = await _authManager.GetAdminAsync();

            admin.AuthDataCurrentOrganId = request.OrganId;

            await _administratorRepository.UpdateCurrentOragnAsync(admin);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
