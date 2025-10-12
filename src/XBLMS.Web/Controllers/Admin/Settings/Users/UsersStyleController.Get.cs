using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersStyleController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var admin = await _authManager.GetAdminAsync();
 

            var organId = admin.CompanyId;

            var list = await _tableStyleRepository.GetUserStylesAsync();
            if (list != null && list.Count > 0)
            {
                foreach (var style in list)
                {
                    style.Set("TypeName", style.InputType.GetDisplayName());
                }
            }

            return new GetResult
            {
                Styles = list,
                TableName = _userRepository.TableName,
                RelatedIdentities = _tableStyleRepository.EmptyRelatedIdentities
            };
        }
    }
}
