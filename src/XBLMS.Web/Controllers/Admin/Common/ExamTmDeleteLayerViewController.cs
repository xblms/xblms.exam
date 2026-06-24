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
    public partial class ExamTmDeleteLayerViewController : ControllerBase
    {
        private const string Route = "common/examTmDeleteLayerView";

        private readonly IExamManager _examManager;
        private readonly IStatLogRepository _statLogRepository;

        public ExamTmDeleteLayerViewController(IExamManager examManager, IStatLogRepository statLogRepository)
        {
            _examManager = examManager;
            _statLogRepository = statLogRepository;
        }

    }
}
