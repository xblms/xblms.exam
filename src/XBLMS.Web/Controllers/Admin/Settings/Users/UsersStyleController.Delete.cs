using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersStyleController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<DeleteResult>> Delete([FromBody] DeleteRequest request)
        {
            var admin = await _authManager.GetAdminAsync();
   
            var organId = admin.CompanyId;

            await _tableStyleRepository.DeleteAsync(_userRepository.TableName, 0, request.AttributeName);

            var allAttributes = _userRepository.TableColumns.Select(x => x.AttributeName).ToList();

            var styles = new List<InputStyle>();
            foreach (var style in await _tableStyleRepository.GetUserStylesAsync())
            {
                styles.Add(new InputStyle
                {
                    Id = style.Id,
                    AttributeName = style.AttributeName,
                    DisplayName = style.DisplayName,
                    InputType = style.InputType,
                    Rules = TranslateUtils.JsonDeserialize<List<InputStyleRule>>(style.RuleValues),
                    Taxis = style.Taxis,
                    IsSystem = ListUtils.ContainsIgnoreCase(allAttributes, style.AttributeName)
                });
            }

            return new DeleteResult
            {
                Styles = styles
            };
        }
    }
}
