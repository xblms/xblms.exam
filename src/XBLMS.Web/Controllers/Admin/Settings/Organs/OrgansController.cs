using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
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
        private const string RouteLazy = "settings/organs/lazy";
        private const string RouteLazyCount = RouteLazy + "/count";
        private const string RouteInfo = "settings/organs/info";
        private const string RouteInfoDel = "settings/organs/info/del";

        private readonly IAuthManager _authManager;
        private readonly IConfigRepository _configRepository;
        private readonly IOrganCompanyRepository _companyRepository;
        private readonly IOrganDepartmentRepository _organDepartmentRepository;
        private readonly IOrganManager _organManager;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IUserRepository _userRepository;

        public OrgansController(IAuthManager authManager, IConfigRepository configRepository, IOrganManager organManager, IOrganCompanyRepository companyRepository, IOrganDepartmentRepository organDepartmentRepository, IAdministratorRepository administratorRepository, IUserRepository userRepository)
        {
            _authManager = authManager;
            _configRepository = configRepository;
            _organManager = organManager;
            _companyRepository = companyRepository;
            _organDepartmentRepository = organDepartmentRepository;
            _administratorRepository = administratorRepository;
            _userRepository = userRepository;
        }

        public class GetResult
        {
            public bool Operate { get; set; }
            public List<OrganTree> Organs { get; set; }

        }
        public class GetLazyRequest
        {
            public bool ShowAdminTotal { get; set; }
            public bool ShowUserTotal { get; set; }
            public string KeyWords { get; set; }
            public int ParentId { get; set; }
            public string OrganType { get; set; }
        }
        public class GetRequest
        {
            public string KeyWord { get; set; }
        }
        public class GetInfoRequest
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string UserType { get; set; }
        }
        public class GetCountResult
        {
            public int Total { get; set; }
            public int Count { get; set; }

        }
        public class GetInfoResult
        {
            public int Id { get; set; }
            public int ParentId { get; set; }
            public string Name { get; set; }
            public string FullName { get; set; }
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
