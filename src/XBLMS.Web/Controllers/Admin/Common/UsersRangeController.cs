using System.Collections.Generic;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Common
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class UsersRangeController : ControllerBase
    {
        private const string Route = "common/usersRange";
        private const string RouteOtherData = Route+"/actions/otherData";
        private const string RouteRange = Route + "/range";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IOrganManager _organManager;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamManager _examManager;
        private readonly IExamPaperRepository _examPaperRepository;

        public UsersRangeController(IAuthManager authManager,
            IPathManager pathManager,
            IDatabaseManager databaseManager,
            IUserRepository userRepository,
            IUserGroupRepository userGroupRepository,
            IOrganManager organManager,
            IExamManager examManager,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperRepository examPaperRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _databaseManager = databaseManager;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _organManager = organManager;
            _examManager = examManager;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperRepository = examPaperRepository;
        }

        public class GetRequest
        {
            public int Id { get; set; }
            public int OrganId { get; set; }
            public string OrganType { get; set; }
            public RangeType RangeType { get; set; }
            public string Keyword { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }

        public class GetResults
        {
            public string Title { get; set; }
            public List<OrganTree> Organs { get; set; }
            public List<User> List { get; set; }
            public int Total { get; set; }
        }


        public class GetRangeRequest
        {
            public int Id { get; set; }
            public RangeType RangeType { get; set; }
            public List<int> Ids { get; set; }
        }
    }
}
