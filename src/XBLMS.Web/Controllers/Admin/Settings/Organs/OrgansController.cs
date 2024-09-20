using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Core.Repositories;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Organs
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class OrgansController : ControllerBase
    {
        private const string Route = "settings/organs";
        private const string RouteInfo = "settings/organs/info";
        private const string RouteInfoDel = "settings/organs/info/del";

        private readonly IAuthManager _authManager;
        private readonly IOrganCompanyRepository _companyRepository;
        private readonly IOrganDepartmentRepository _organDepartmentRepository;
        private readonly IOrganDutyRepository _organDutyRepository;
        private readonly IOrganManager _organManager;

        public OrgansController(IAuthManager authManager, IOrganManager organManager, IOrganCompanyRepository companyRepository, IOrganDepartmentRepository organDepartmentRepository, IOrganDutyRepository organDutyRepository)
        {
            _authManager = authManager;
            _organManager = organManager;
            _companyRepository = companyRepository;
            _organDepartmentRepository = organDepartmentRepository;
            _organDutyRepository = organDutyRepository;
        }

        public class GetResult
        {
            public List<OrganTree> Organs { get; set; }

        }
        public class GetRequest
        {
            public string KeyWord { get; set; }
        }
        public class GetInfoRequest
        {
            public int Id { get; set; }
            public string Type { get; set; }
        }
        public class GetInfoResult
        {
            public int Id { get; set; }
            public int ParentId { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }

        }
        public class GetSubmitRequest : GetInfoResult
        {
            public string ParentType { get; set; }
        }

        public class GetDeleteRequest
        {
            public OrganTree Organs { get; set; }
        }
    }
}
