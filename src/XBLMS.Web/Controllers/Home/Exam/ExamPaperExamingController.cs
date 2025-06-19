﻿using Microsoft.AspNetCore.Authorization;
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
    public partial class ExamPaperExamingController : ControllerBase
    {
        private const string Route = "exam/examPaperExaming";
        private const string RouteSubmitAnswer = Route + "/submitAnswer";
        private const string RouteSubmitAnswerSmall = Route + "/submitAnswerSmall";
        private const string RouteSubmitPaper = Route + "/submitPaper";
        private const string RouteSubmitTiming = Route + "/submitTiming";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly ICreateManager _createManager;
        private readonly IOrganManager _organManager;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperRandomConfigRepository _examPaperRandomConfigRepository;
        private readonly IExamPaperRandomRepository _examPaperRandomRepository;
        private readonly IExamPaperRandomTmRepository _examPaperRandomTmRepository;
        private readonly IExamPaperRandomTmSmallRepository _examPaperRandomTmSmallRepository;
        private readonly IExamPaperAnswerRepository _examPaperAnswerRepository;
        private readonly IExamPaperAnswerSmallRepository _examPaperAnswerSmallRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamManager _examManager;
        private readonly IExamTxRepository _examTxRepository;

        public ExamPaperExamingController(IConfigRepository configRepository,
            ICreateManager createManager,
            IAuthManager authManager,
            IExamPaperRepository examPaperRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperRandomConfigRepository examPaperRandomConfigRepository,
            IExamPaperRandomRepository examPaperRandomRepository,
            IExamPaperRandomTmRepository examPaperRandomTmRepository,
            IExamManager examManager,
            IExamPaperAnswerRepository examPaperAnswerRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IOrganManager organManager,
            IExamTxRepository examTxRepository,
            IExamPaperAnswerSmallRepository examPaperAnswerSmallRepository,
            IExamPaperRandomTmSmallRepository examPaperRandomTmSmallRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _createManager = createManager;
            _examPaperRepository = examPaperRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperRandomConfigRepository = examPaperRandomConfigRepository;
            _examPaperRandomRepository = examPaperRandomRepository;
            _examPaperRandomTmRepository = examPaperRandomTmRepository;
            _examManager = examManager;
            _examPaperAnswerRepository = examPaperAnswerRepository;
            _examPaperStartRepository = examPaperStartRepository;
            _organManager = organManager;
            _examTxRepository = examTxRepository;
            _examPaperAnswerSmallRepository = examPaperAnswerSmallRepository;
            _examPaperRandomTmSmallRepository = examPaperRandomTmSmallRepository;
        }
        public class GetRequest
        {
            public int Id { get; set; }
            public int LoadCounts { get; set; }
        }
        public class GetResult
        {
            public string Watermark { get; set; }
            public ExamPaper Item { get; set; }
            public List<ExamPaperRandomConfig> TxList { get; set; }
        }
        public class GetSubmitAnswerRequest
        {
            public ExamPaperAnswer Answer { get; set; }
        }
        public class GetSubmitAnswerSmallRequest
        {
            public ExamPaperAnswerSmall Answer { get; set; }
        }
    }
}
