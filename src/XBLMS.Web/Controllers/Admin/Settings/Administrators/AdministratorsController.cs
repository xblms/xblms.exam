using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class AdministratorsController : ControllerBase
    {
        private const string Route = "settings/administrators";
        private const string RouteOtherData = "settings/administrators/actions/otherData";
        private const string RouteDelete = "settings/administrators/actions/delete";
        private const string RouteLock = "settings/administrators/actions/lock";
        private const string RouteUnLock = "settings/administrators/actions/unLock";

        private readonly ICacheManager _cacheManager;
        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAdministratorsInRolesRepository _administratorsInRolesRepository;
        private readonly IOrganManager _organManager;

        public AdministratorsController(ICacheManager cacheManager, IAuthManager authManager, IPathManager pathManager, IDatabaseManager databaseManager, IAdministratorRepository administratorRepository, IRoleRepository roleRepository, IOrganManager organManager, IAdministratorsInRolesRepository administratorsInRolesRepository)
        {
            _cacheManager = cacheManager;
            _authManager = authManager;
            _pathManager = pathManager;
            _databaseManager = databaseManager;
            _administratorRepository = administratorRepository;
            _roleRepository = roleRepository;
            _administratorsInRolesRepository = administratorsInRolesRepository;
            _organManager = organManager;
        }

        public class GetRequest
        {
            public int OrganId { get; set; }
            public string OrganType { get; set; }
            public string Role { get; set; }
            public string Order { get; set; }
            public int LastActivityDate { get; set; }
            public string Keyword { get; set; }
            public int Offset { get; set; }
            public int Limit { get; set; }
        }

        public class Admin
        {
            public int Id { get; set; }
            public string Guid { get; set; }
            public string AvatarUrl { get; set; }
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public string Mobile { get; set; }
            public DateTime? LastActivityDate { get; set; }
            public int CountOfLogin { get; set; }
            public bool Locked { get; set; }
            public string Roles { get; set; }
            public AuthorityType Auth { get; set; }
        }

        public class GetResult
        {
            public List<Admin> Administrators { get; set; }
            public List<OrganTree> Organs { get; set; }
            public int Count { get; set; }
            public List<Select<string>> Roles { get; set; }
            public int AdminId { get; set; }
        }
    }
}
