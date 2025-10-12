using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Study
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class StudyCourseEvaluationLayerViewController : ControllerBase
    {
        private const string Route = "study/studyCourseEvaluationLayerView";

        private readonly IAuthManager _authManager;

        private readonly IStudyCourseEvaluationRepository _studyCourseEvaluationRepository;
        private readonly IStudyCourseEvaluationItemRepository _studyCourseEvaluationItemRepository;

        public StudyCourseEvaluationLayerViewController(IAuthManager authManager, IStudyCourseEvaluationRepository studyCourseEvaluationRepository, IStudyCourseEvaluationItemRepository studyCourseEvaluationItemRepository)
        {
            _authManager = authManager;
            _studyCourseEvaluationRepository = studyCourseEvaluationRepository;
            _studyCourseEvaluationItemRepository = studyCourseEvaluationItemRepository;
        }

        public class GetResult
        {
            public StudyCourseEvaluation Item { get; set; }
            public List<StudyCourseEvaluationItem> ItemList { get; set; }
        }

    }
}
