using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    public partial class AddController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();

            var rule = await _ruleRepository.GetAsync(request.RuleId) ?? new BlockRule
            {
                AreaType = AreaType.None,
                AllowList = new List<string>(),
                BlockList = new List<string>(),
                BlockMethod = BlockMethod.Warning
            };

            var areas = _blockManager.GetAreas();
            var blockAreas = new List<IdName>();
            if (rule.BlockAreas != null && areas != null)
            {
                blockAreas = areas.Where(x => rule.BlockAreas.Contains(x.Id)).ToList();
            }

            return new GetResult
            {
                Rule = rule,
                AreaTypes = ListUtils.GetSelects<AreaType>(),
                BlockAreas = blockAreas,
            };
        }
    }
}
