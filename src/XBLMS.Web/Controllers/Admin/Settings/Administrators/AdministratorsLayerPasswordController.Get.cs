using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Core.Utils;
using XBLMS.Configuration;
using XBLMS.Utils;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsLayerPasswordController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var userName = request.UserName;
            var adminName = _authManager.AdminName;

            if (string.IsNullOrEmpty(userName)) userName = adminName;
            var administrator = await _administratorRepository.GetByUserNameAsync(userName);
            if (administrator == null) return this.Error(Constants.ErrorNotFound);

            return new GetResult
            {
                Administrator = administrator,
                OldPassword = request.UserName == _authManager.AdminName,
            };
        }
    }
}
