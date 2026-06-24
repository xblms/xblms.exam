using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Common
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class SelectAdministratorsController : ControllerBase
    {
        private const string Route = "common/selectAdministrators";

        private readonly IAuthManager _authManager;
        private readonly IAdministratorRepository _administratorRepository;

        public SelectAdministratorsController(IAuthManager authManager,IAdministratorRepository administratorRepository)
        {
            _authManager = authManager;
            _administratorRepository = administratorRepository;
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
            public string Organ { get; set; }
        }

        public class GetResult
        {
            public List<Admin> Administrators { get; set; }
            public int Count { get; set; }
        }
    }
}
