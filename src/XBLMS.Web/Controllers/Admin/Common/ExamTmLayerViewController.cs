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
        private const string RouteCorrection = "common/examTmCorrectionLayerView";

        private readonly IExamTmRepository _examTmRepository;
        private readonly IExamManager _examManager;
        private readonly ITableStyleRepository _tableStyleRepository;
        private readonly IExamTmCorrectionRepository _examTmCorrectionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IExamTxRepository _examTxRepository;

        public ExamTmLayerViewController(IExamTmRepository examTmRepository, IExamManager examManager, ITableStyleRepository tableStyleRepository, IExamTmCorrectionRepository examTmCorrectionRepository, IUserRepository userRepository, IExamTxRepository examTxRepository)
        {
            _examTmRepository = examTmRepository;
            _examManager = examManager;
            _examTmCorrectionRepository = examTmCorrectionRepository;
            _tableStyleRepository = tableStyleRepository;
            _userRepository = userRepository;
            _examTxRepository = examTxRepository;
        }
        public class GetResult
        {
            public List<Models.TableStyle> Styles { get; set; }
            public ExamTm Item { get; set; }
        }
        public class GetCorrectionResult
        {
            public ExamTm ItemNew { get; set; }
            public ExamTm Item { get; set; }
            public ExamTmCorrection Info { get; set; }
        }
    }
}
