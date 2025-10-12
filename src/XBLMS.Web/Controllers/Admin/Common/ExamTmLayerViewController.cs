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
    public partial class ExamTmLayerViewController : ControllerBase
    {
        private const string Route = "common/examTmLayerView";

        private readonly IExamTmRepository _examTmRepository;
        private readonly IAuthManager _authManager;
        private readonly IExamManager _examManager;
        private readonly ITableStyleRepository _tableStyleRepository;

        public ExamTmLayerViewController(IAuthManager authManager, IExamTmRepository examTmRepository, IExamManager examManager, ITableStyleRepository tableStyleRepository)
        {
            _authManager = authManager;
            _examTmRepository = examTmRepository;
            _examManager = examManager;
            _tableStyleRepository = tableStyleRepository;
        }
        public class GetResult
        {
            public List<Models.TableStyle> Styles { get; set; }
            public ExamTm Item { get; set; }
        }
    }
}
