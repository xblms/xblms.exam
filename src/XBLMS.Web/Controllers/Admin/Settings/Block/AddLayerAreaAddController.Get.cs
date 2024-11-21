using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    public partial class AddLayerAreaAddController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var admin = await _authManager.GetAdminAsync();

            var areas = _blockManager.GetAreas();

            return new GetResult
            {
                Areas = areas.Select(x => new Select<int>
                {
                    Value = x.Id,
                    Label = x.Name
                }).ToList()
            };
        }
    }
}
