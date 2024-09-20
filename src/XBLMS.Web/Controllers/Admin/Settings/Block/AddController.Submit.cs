using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    public partial class AddController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] SubmitRequest request)
        {
            if (request.Rule.Id > 0)
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
                {
                    return this.NoAuth();
                }
            }
            else
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
                {
                    return this.NoAuth();
                }
            }


            var admin = await _authManager.GetAdminAsync();

            if (request.Rule.Id == 0)
            {
                if (await _ruleRepository.IsExistsAsync(request.Rule.RuleName))
                {
                    return this.Error("保存失败，已存在相同名称的拦截规则！");
                }
            }

            var rule = request.Rule;
            rule.BlockAreas = request.BlockAreas.Select(x => x.Id).ToList();

            if (rule.Id > 0)
            {
                await _ruleRepository.UpdateAsync(rule);
            }
            else
            {
                await _ruleRepository.InsertAsync(rule);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
