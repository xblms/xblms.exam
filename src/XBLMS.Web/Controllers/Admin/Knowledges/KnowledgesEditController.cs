using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class KnowledgesEditController : ControllerBase
    {
        private const string Route = "knowledges/edit";
        private const string RouteUpload = Route + "/upload";

        private readonly IAuthManager _authManager;
        private readonly IKnowlegesRepository _knowlegesRepository;
        private readonly IUploadManager _uploadManager;

        public KnowledgesEditController(IAuthManager authManager,
            IKnowlegesRepository knowlegesRepository,
            IUploadManager uploadManager)
        {
            _authManager = authManager;
            _knowlegesRepository = knowlegesRepository;
            _uploadManager = uploadManager;
        }
        public class GetResult
        {
            public GetItemInfo Item { get; set; }
        }
        public class GetItemInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string CoverImgUrl { get; set; }
            public bool OnlyCompany { get; set; }
        }
        public class GetSubmitRequest
        {
            public GetItemInfo Item { get; set; }
        }

        public class GetUploadResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public string CoverImagePath { get; set; }
        }
    }
}
