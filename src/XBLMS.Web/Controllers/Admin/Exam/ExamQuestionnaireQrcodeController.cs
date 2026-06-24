using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Repositories;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamQuestionnaireQrcodeController : ControllerBase
    {
        private const string Route = "exam/examQuestionnaireQrcode";

        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;

        public ExamQuestionnaireQrcodeController(IExamQuestionnaireRepository examQuestionnaireRepository)
        {
            _examQuestionnaireRepository = examQuestionnaireRepository;
        }
        public class GetResult
        {
            public string Title { get; set; }
            public string QrcodeUrl { get; set; }
        }
    }
}
