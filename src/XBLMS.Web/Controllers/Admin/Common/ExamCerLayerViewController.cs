using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Repositories;

namespace XBLMS.Web.Controllers.Admin.Common
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamCerLayerViewController : ControllerBase
    {
        private const string Route = "common/examCerLayerView";

        private readonly IExamCerRepository _examCerRepository;
        private readonly IExamCerUserRepository _examCerUserRepository;

        public ExamCerLayerViewController(IExamCerRepository examCerRepository, IExamCerUserRepository examCerUserRepository)
        {
            _examCerRepository = examCerRepository;
            _examCerUserRepository = examCerUserRepository;
        }

    }
}
