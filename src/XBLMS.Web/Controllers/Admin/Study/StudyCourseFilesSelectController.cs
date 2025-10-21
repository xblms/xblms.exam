using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;
namespace XBLMS.Web.Controllers.Admin.Study
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class StudyCourseFilesSelectController : ControllerBase
    {
        private const string Route = "study/studyCourseFilesSelect";

        private readonly IAuthManager _authManager;
        private readonly IStudyCourseFilesRepository _studyCourseFilesRepository;
        private readonly IStudyCourseFilesGroupRepository _studyCourseFilesGroupRepository;
        private readonly IPathManager _pathManager;

        public StudyCourseFilesSelectController(IAuthManager authManager, IStudyCourseFilesRepository studyCourseFilesRepository, IPathManager pathManager, IStudyCourseFilesGroupRepository studyCourseFilesGroupRepository)
        {
            _authManager = authManager;
            _studyCourseFilesRepository = studyCourseFilesRepository;
            _pathManager = pathManager;
            _studyCourseFilesGroupRepository = studyCourseFilesGroupRepository;
        }

        public class GetRequest
        {
            public string FileType { get; set; }
            public string Keyword { get; set; }
            public int GroupId { get; set; }
        }

        public class GetQueryResult
        {
            public List<GetListInfo> List { get; set; }
            public IEnumerable<GetQueryResultPath> Paths { get; set; }
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
            public string CoverView { get; set; }
            public string Url { get; set; }
            public bool IsVideo { get; set; }
        }
        public class GetQueryResultPath
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
