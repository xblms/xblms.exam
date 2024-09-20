using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Authorization;
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

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IUploadManager _uploadManager;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IOrganManager _organManager;

        public AdministratorsLayerProfileController(IAuthManager authManager, IPathManager pathManager,
            IOrganManager organManager,
            IAdministratorRepository administratorRepository, IUploadManager uploadManager)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _administratorRepository = administratorRepository;
            _organManager = organManager;
            _uploadManager = uploadManager;
        }

        public class GetRequest
        {
            public string UserName { get; set; }
        }
        public class GetResult
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public string AvatarUrl { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string Auth { get; set; }
            public string OrganId { get; set; }
            public List<Select<string>> Auths { get; set; }
            public List<OrganTree> Organs { get; set; }
            public List<OrganTree> AuthOrgans { get; set; }
            public bool AuthIsWithChildren { get; set; }
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
            public string OrganId { get; set; }
            public bool AuthIsWithChildren { get; set; }
            public List<int> AuthOrganIds { get; set; }
        }
    }
}
