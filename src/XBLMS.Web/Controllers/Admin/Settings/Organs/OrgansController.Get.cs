using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Settings.Organs
{
    public partial class OrgansController
    {
        [HttpGet, Route(RouteLazy)]
        public async Task<ActionResult<GetResult>> GetLazy([FromQuery] GetLazyRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var parentId = request.ParentId;

            List<OrganTree> organs;

            if (!string.IsNullOrEmpty(request.KeyWords))
            {
                organs = await _organManager.GetOrganTreeTableDataLazySearchAsync(adminAuth, request.KeyWords, request.ShowAdminTotal, request.ShowUserTotal);
            }
            else
            {
                organs = await _organManager.GetOrganTreeTableDataLazyAsync(adminAuth, parentId, request.OrganType, request.ShowAdminTotal, request.ShowUserTotal);
            }

            return new GetResult
            {
                Operate = adminAuth.AuthDataType != Enums.AuthorityDataType.DataCreator,
                Organs = organs,
            };
        }


        [HttpGet, Route(RouteInfo)]
        public async Task<ActionResult<GetInfoResult>> GetInfo([FromQuery] GetInfoRequest request)
        {
            var result = new GetInfoResult();

            if (request.Type == "company")
            {
                var company = await _companyRepository.GetAsync(request.Id);
                result.Id = company.Id;
                result.Name = company.Name;
            }
            if (request.Type == "department")
            {
                var department = await _organDepartmentRepository.GetAsync(request.Id);
                result.Id = department.Id;
                result.Name = department.Name;
            }
            return result;
        }
        [HttpGet, Route(RouteLazyCount)]
        public async Task<ActionResult<GetCountResult>> GetCount([FromQuery] GetInfoRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var result = new GetInfoResult();
            var total = 0;
            var count = 0;
            if (request.Type == "company")
            {
                if (request.UserType == "admin")
                {
                    (total, count) = await _administratorRepository.GetCountByCompanyAsync(adminAuth, request.Id);
                }
                else
                {
                    (total, count) = await _userRepository.GetCountByCompanyAsync(adminAuth, request.Id);
                }

            }
            if (request.Type == "department")
            {
                if (request.UserType == "admin")
                {
                    (total, count) = await _administratorRepository.GetCountByDepartmentAsync(adminAuth, request.Id);
                }
                else
                {
                    (total, count) = await _userRepository.GetCountByDepartmentAsync(adminAuth, request.Id);
                }
            }

            return new GetCountResult
            {
                Total = total,
                Count = count
            };
        }
    }
}
