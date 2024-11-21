using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UsersRangeController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResults>> Get([FromQuery] GetRequest request)
        {
            var companyIds = new List<int>();
            var departmentIds = new List<int>();
            var dutyIds = new List<int>();
            if (request.OrganId > 0)
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

            var (total, list) = await _userRepository.GetListAsync(companyIds, departmentIds, dutyIds, request.Keyword, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    await _organManager.GetUser(item);
                    var isRange = false;
                    if (request.RangeType == RangeType.Exam)
                    {
                        isRange = await _examPaperUserRepository.ExistsAsync(request.Id, item.Id);
                    }
                    item.Set("IsRange", isRange);
                }
            }


            return new GetResults
            {
                List = list,
                Total = total,
            };
        }

        [HttpGet, Route(RouteOtherData)]
        public async Task<ActionResult<GetResults>> GetOtherData([FromQuery] GetRequest request)
        {
            var organs = await _organManager.GetOrganTreeTableDataAsync();

            var title = "请选择用户";
            if (request.RangeType == RangeType.Exam)
            {
                var paper = await _examPaperRepository.GetAsync(request.Id);
                if (paper != null)
                {
                    title = $"{paper.Title}-安排考生";
                }
            }

            return new GetResults
            {
                Title = title,
                Organs = organs,
            };
        }
    }
}
