using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class UsersController : ControllerBase
    {
        private const string Route = "settings/users";
        private const string RouteOtherData = "settings/users/actions/otherData";
        private const string RouteDelete = "settings/users/actions/delete";
        private const string RouteDeletes = "settings/users/actions/deletes";
        private const string RouteImportCheck = "settings/users/actions/importCheck";
        private const string RouteImport = "settings/users/actions/import";
        private const string RouteExport = "settings/users/actions/export";
        private const string RouteCheck = "settings/users/actions/check";
        private const string RouteLock = "settings/users/actions/lock";
        private const string RouteUnLock = "settings/users/actions/unLock";

        private const string RouteDoc = Route + "/doc";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IOrganManager _organManager;
        private readonly IExamManager _examManager;
        private readonly IConfigRepository _configRepository;

        public UsersController(IAuthManager authManager, IPathManager pathManager, IDatabaseManager databaseManager, IUserRepository userRepository, IUserGroupRepository userGroupRepository, IOrganManager organManager, IExamManager examManager, IConfigRepository configRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _databaseManager = databaseManager;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _organManager = organManager;
            _examManager = examManager;
            _configRepository = configRepository;
        }

        public class GetRequest
        {
            public int OrganId { get; set; }
            public string OrganType { get; set; }
            public int GroupId { get; set; }
            public string Order { get; set; }
            public int LastActivityDate { get; set; }
            public string Keyword { get; set; }
            public int Offset { get; set; }
            public int Limit { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
        }

        public class GetResults
        {
            public List<User> Users { get; set; }
            public int Count { get; set; }
            public List<UserGroup> Groups { get; set; }
            public SystemCode SystemCode { get; set; } 
        }
        public class ImportCheckResult
        {
            public bool Value { get; set; }
            public List<KeyValuePair<int, string>> Msgs { get; set; }
            public string FilePath { get; set; }
            public int Success { set; get; }
            public int Failure { set; get; }
        }
        public class ImportRequest
        {
            public List<int> RowNumber { get; set; }
            public string FilePath { get; set; }
        }
        public class ImportResult
        {
            public int Success { set; get; }
            public int Failure { set; get; }
        }

        public class GetDeletesRequest
        {
            public List<int> Ids { get; set; }
        }
    }
}
