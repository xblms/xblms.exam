using System.Threading.Tasks;
using AngleSharp.Dom;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Organs
{
    public partial class OrgansController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }
            var organs = await _organManager.GetOrganTreeTableDataAsync();

            return new GetResult
            {
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
            if (request.Type == "duty")
            {
                var duty = await _organDutyRepository.GetAsync(request.Id);
                result.Id = duty.Id;
                result.Name = duty.Name;
            }
            return result;
        }
    }
}
