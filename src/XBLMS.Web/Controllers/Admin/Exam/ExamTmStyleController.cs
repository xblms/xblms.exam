using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamTmStyleController : ControllerBase
    {
        private const string Route = "exam/examTmStyle";
        private const string RouteDelete = Route + "/del";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly IExamTmRepository _examTmRepository;
        private readonly ITableStyleRepository _tableStyleRepository;

        public ExamTmStyleController(IAuthManager authManager, IPathManager pathManager, IDatabaseManager databaseManager, IExamTmRepository examTmRepository, ITableStyleRepository tableStyleRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _databaseManager = databaseManager;
            _examTmRepository = examTmRepository;
            _tableStyleRepository = tableStyleRepository;
        }

        public class GetResult
        {
            public List<Models.TableStyle> Styles { get; set; }
            public string TableName { get; set; }
            public List<int> RelatedIdentities { get; set; }
        }

        public class DeleteRequest
        {
            public string AttributeName { get; set; }
        }

        public class DeleteResult
        {
            public List<InputStyle> Styles { get; set; }
        }

        public class ResetResult
        {
            public List<InputStyle> Styles { get; set; }
        }
    }
}
