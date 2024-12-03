using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Common
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamAssessmentConfigLayerViewController : ControllerBase
    {
        private const string Route = "common/examAssessmentConfigLayerView";

        private readonly IAuthManager _authManager;

        private readonly IExamAssessmentConfigSetRepository _examAssessmentConfigSetRepository;
        private readonly IExamAssessmentConfigRepository _examAssessmentConfigRepository;

        public ExamAssessmentConfigLayerViewController(IAuthManager authManager, IExamAssessmentConfigSetRepository examAssessmentConfigSetRepository, IExamAssessmentConfigRepository examAssessmentConfigRepository)
        {
            _authManager = authManager;
            _examAssessmentConfigSetRepository = examAssessmentConfigSetRepository;
            _examAssessmentConfigRepository = examAssessmentConfigRepository;
        }

        public class GetResult
        {
            public ExamAssessmentConfig Item { get; set; }
            public List<ExamAssessmentConfigSet> ItemList { get; set; }
        }

    }
}
