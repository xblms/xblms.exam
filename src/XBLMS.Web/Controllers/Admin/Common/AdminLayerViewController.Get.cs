using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class AdminLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            Administrator admin = null;
            if (request.Id > 0)
            {
                admin = await _administratorRepository.GetByUserIdAsync(request.Id);
            }

            if (admin == null) return this.Error(Constants.ErrorNotFound);

            admin = await _organManager.GetAdministrator(admin.Id);

            return new GetResult
            {
                Administrator = admin,
            };
        }
    }
}
