using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmGroupRangeController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResults>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var companyIds = new List<int>();
            var departmentIds = new List<int>();
            var dutyIds = new List<int>();
            if (request.OrganId == 0) { }
            else if (request.OrganId == 1 && request.OrganType == "company") { }
            else
            {
                if (request.OrganId != 0)
                {
                    if (request.OrganType == "company")
                    {
                        companyIds = await _organManager.GetCompanyIdsAsync(request.OrganId);
                    }
                    if (request.OrganType == "department")
                    {
                        departmentIds = await _organManager.GetDepartmentIdsAsync(request.OrganId);
                    }
                    if (request.OrganType == "duty")
                    {
                        dutyIds = await _organManager.GetDutyIdsAsync(request.OrganId);
                    }
                }
            }

            var group = await _userGroupRepository.GetAsync(request.GroupId);


            var count = await _userRepository.GetCountAsync(companyIds, departmentIds, dutyIds, group.UserIds, request.Range, request.LastActivityDate, request.Keyword);
            var users = await _userRepository.GetUsersAsync(companyIds, departmentIds, dutyIds, group.UserIds, request.Range, request.LastActivityDate, request.Keyword, request.Order, request.Offset, request.Limit);

            return new GetResults
            {
                Users = users,
                Count = count,
                GroupName = group.GroupName
            };
        }

        [HttpGet, Route(RouteOtherData)]
        public async Task<ActionResult<GetResults>> GetOtherData()
        {
            var organs = await _organManager.GetOrganTreeTableDataAsync();
            return new GetResults
            {
                Organs = organs,
            };
        }
    }
}
