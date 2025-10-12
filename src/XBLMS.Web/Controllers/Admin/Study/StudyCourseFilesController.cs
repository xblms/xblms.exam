using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Study
{
    [OpenApiIgnore]
    [Route(Constants.ApiAdminPrefix)]
    public partial class StudyCourseFilesController : ControllerBase
    {
        private const string Route = "study/studyCourseFiles";
        private const string RouteDelete = Route + "/file/del";
        private const string RouteActionsDeleteGroup = Route + "/group/del";
        private const string RouteActionsDeleteGroupAndFile = Route + "/delList";
        private const string RouteActionsDownload = Route + "/file/download";

        private readonly ISettingsManager _settingsManager;
        private readonly ICreateManager _createManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IStudyCourseFilesRepository _studyCourseFilesRepository;
        private readonly IStudyCourseFilesGroupRepository _studyCourseFilesGroupRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IStatRepository _statRepository;
        private readonly IOrganManager _organManager;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IDbCacheRepository _dbCacheRepository;
        public StudyCourseFilesController(IDatabaseManager databaseManager,
            ISettingsManager settingsManager,
            ICreateManager createManager,
            IAuthManager authManager,
            IPathManager pathManager,
            IConfigRepository configRepository,
            IStatRepository statRepository,
            IAdministratorRepository administratorRepository,
            IDbCacheRepository dbCacheRepository,
            IStudyCourseFilesRepository studyCourseFilesRepository,
            IStudyCourseFilesGroupRepository studyCourseFilesGroupRepository,
            IOrganManager organManager)
        {
            _databaseManager = databaseManager;
            _settingsManager = settingsManager;
            _createManager = createManager;
            _authManager = authManager;
            _pathManager = pathManager;
            _configRepository = configRepository;
            _statRepository = statRepository;
            _administratorRepository = administratorRepository;
            _dbCacheRepository = dbCacheRepository;
            _studyCourseFilesRepository = studyCourseFilesRepository;
            _studyCourseFilesGroupRepository = studyCourseFilesGroupRepository;
            _organManager = organManager;
        }

        public class GetResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
        }

        public class FileRedirectToken
        {
            public string UserName { get; set; }
            public string Timespan { get; set; }
        }

        public class GetRequest
        {
            public string Keyword { get; set; }
            public int GroupId { get; set; }
            public string Token { get; set; }
            public bool IsFileServer { get; set; }
        }

        public class GetQueryResult
        {
            public List<GetListInfo> List { get; set; }
            public IEnumerable<GetQueryResultPath> Paths { get; set; }
            public string Token { get; set; } = "";
            public string SessionId { get; set; } = "";
            public string RedirectUrl { get; set; } = "";
        }
        public class GetListInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string FileType { get; set; }
            public long Size { get; set; }
            public string DateTimeStr { get; set; }
            public int Duration { get; set; }
            public string Cover { get; set; }
            public bool IsVideo { get; set; }
        }
        public class GetQueryResultPath
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class CreateRequest
        {
            public int GroupId { get; set; }
            public int Duration { get; set; }
            public string Cover { get; set; }
        }

        public class DownloadRequest
        {
            public int Id { get; set; }
        }

        public class UpdateReques
        {
            public int Id { get; set; }

            public string Title { get; set; }

            public int GroupId { get; set; }
        }

        public class DeleteRequest
        {
            public int Id { get; set; }
        }

        public class DeleteGroupRequest
        {
            public int Id { get; set; }
        }
        public class DeleteGroupAndFileRequest
        {
            public List<GetFileInfo> Files { get; set; }
        }
        public class GetFileInfo
        {
            public int Id { get; set; }
            public string Type { get; set; }
        }

        public class PullRequest
        {
            public int GroupId { get; set; }
        }

        public class GetOrganResult
        {
            public List<GetOrganInfo> List { get; set; }
        }
        public class GetOrganInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class GetShareFileRequest
        {
            public List<GetFileInfo> Files { get; set; }
            public List<int> OrganIds { get; set; }
        }
    }
}
