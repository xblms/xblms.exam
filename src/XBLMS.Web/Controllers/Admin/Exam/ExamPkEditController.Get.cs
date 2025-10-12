using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPkEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();

            var pk = new ExamPk();
            pk.Name = $"答题竞赛-{StringUtils.GetShortGuid()}";
            pk.RuleType = PkRuleType.Game1;

            if (request.Id > 0)
            {
                pk = await _examPkRepository.GetAsync(request.Id);
            }

            var userGroups = await _userGroupRepository.GetListAsync(adminAuth, true);
            var tmGroups = await _tmGroupRepository.GetListAsync(adminAuth, string.Empty, true);

            return new GetResult
            {
                Item = pk,
                UserGroupList = userGroups,
                TmGroupList = tmGroups,
                RuleList = ListUtils.GetSelects<PkRuleType>()
            };

        }

    }
}
