using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Common
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamCerLayerViewController : ControllerBase
    {
        private const string Route = "common/examCerLayerView";

        private readonly IAuthManager _authManager;
        private readonly IExamCerRepository _examCerRepository;
        private readonly IExamCerUserRepository _examCerUserRepository;

        public ExamCerLayerViewController(IAuthManager authManager, IExamCerRepository examCerRepository, IExamCerUserRepository examCerUserRepository)
        {
            _authManager = authManager;
            _examCerRepository = examCerRepository;
            _examCerUserRepository = examCerUserRepository;
        }

    }
}
