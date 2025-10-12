using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;
namespace XBLMS.Web.Controllers.Admin.Study
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class StudyCourseFileLayerViewController : ControllerBase
    {
        private const string Route = "study/studyCourseFileLayerView";

        private readonly IAuthManager _authManager;
        private readonly IStudyCourseFilesRepository _studyCourseFilesRepository;
        private readonly IPathManager _pathManager;

        public StudyCourseFileLayerViewController(IAuthManager authManager, IStudyCourseFilesRepository studyCourseFilesRepository, IPathManager pathManager)
        {
            _authManager = authManager;
            _studyCourseFilesRepository = studyCourseFilesRepository;
            _pathManager = pathManager;
        }

        public class GetResult
        {
            public string FileName { get; set; }
            public string FileUrl { get; set; }
            public string FileType { get; set; }
            public bool IsVideo { get; set; }
        }
    }
}
