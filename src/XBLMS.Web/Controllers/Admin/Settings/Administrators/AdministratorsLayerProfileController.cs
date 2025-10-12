﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
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
    public partial class AdministratorsLayerProfileController : ControllerBase
    {
        private const string Route = "settings/administratorsLayerProfile";
        private const string RouteUpload = "settings/administratorsLayerProfile/actions/upload";
        private const string RouteRoles = "settings/administratorsLayerProfile/actions/roles";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IUploadManager _uploadManager;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IAdministratorsInRolesRepository _administratorsInRolesRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IOrganManager _organManager;

        public AdministratorsLayerProfileController(IAuthManager authManager, IPathManager pathManager,
            IOrganManager organManager,
            IRoleRepository roleRepository,
            IAdministratorsInRolesRepository administratorsInRolesRepository,
            IAdministratorRepository administratorRepository, IUploadManager uploadManager)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _administratorsInRolesRepository = administratorsInRolesRepository;
            _administratorRepository = administratorRepository;
            _roleRepository = roleRepository;
            _organManager = organManager;
            _uploadManager = uploadManager;
        }

        public class GetRequest
        {
            public string UserName { get; set; }
        }
        public class GetResult
        {
            public bool IsSelf { get; set; }
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public string AvatarUrl { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string Auth { get; set; }
            public string AuthData { get; set; }
            public int OrganId { get; set; }
            public string OrganName { get; set; }
            public string OrganType { get; set; }
            public List<Select<string>> Auths { get; set; }
            public List<Select<string>> AuthDatas { get; set; }
            public List<int> RolesIds { get; set; }
            public int AdminId { get; set; }
            public int CreatorId { get; set; }
        }

        public class SubmitRequest
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public string Password { get; set; }
            public string AvatarUrl { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public AuthorityType Auth { get; set; }
            public AuthorityDataType? AuthData { get; set; }
            public int OrganId { get; set; }
            public string OrganType { get; set; }
            public List<int> RolesIds { get; set; }
        }

        public class GetRolesResult
        {
           public  List<GetRolesResultInfo> List { get; set; }
        }
        public class GetRolesResultInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
