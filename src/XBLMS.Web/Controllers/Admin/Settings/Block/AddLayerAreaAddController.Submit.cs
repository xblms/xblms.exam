using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    public partial class AddLayerAreaAddController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<SubmitResult>> Submit([FromBody] SubmitRequest request)
        {
            var admin = await _authManager.GetAdminAsync();

            var areas = new List<IdName>();
            foreach (var areaId in request.AreaIds)
            {
                var area = _blockManager.GetArea(areaId);

                areas.Add(new IdName
                {
                    Id = areaId,
                    Name = $"{area.AreaEn}({area.AreaCn})"
                });
            }

            return new SubmitResult
            {
                Areas = areas
            };
        }
    }
}
