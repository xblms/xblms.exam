using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class CrudDemoController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetList([FromQuery] GetRequest request)
        {
            if (! await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var (total, list) = await _crudDemoRepository.ListAsync(request.Title, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var creator = await _administratorRepository.GetByUserIdAsync(item.CreatorId);
                    item.Set("CreatorId", creator.Id);
                    item.Set("CreatorName", creator.DisplayName);
                }
            }
            return new GetResult
            {
                Total = total,
                List = list
            };
        }
    }
}
