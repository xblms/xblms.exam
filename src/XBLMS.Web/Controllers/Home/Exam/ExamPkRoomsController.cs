using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class ExamPkRoomsController : ControllerBase
    {
        private const string Route = "exam/examPkRooms";

        private readonly IAuthManager _authManager;
        private readonly IOrganManager _organManager;

        private readonly IExamPkRepository _examPkRepository;
        private readonly IExamPkRoomRepository _examPkRoomRepository;


        public ExamPkRoomsController(IAuthManager authManager,
            IOrganManager organManager,
            IExamPkRepository examPkRepository,
            IExamPkRoomRepository examPkRoomRepository)
        {
            _authManager = authManager;
            _organManager = organManager;
            _examPkRepository = examPkRepository;
            _examPkRoomRepository = examPkRoomRepository;
        }


        public class GetRequest
        {
            public int Id { get; set; }
        }
        public class GetResult
        {
            public string Title { get; set; }
            public List<ExamPk> List { get; set; }
        }
    }
}
