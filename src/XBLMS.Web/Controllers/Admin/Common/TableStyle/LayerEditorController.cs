﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Common.TableStyle
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class LayerEditorController : ControllerBase
    {
        private const string Route = "common/tableStyle/layerEditor";

        private readonly IAuthManager _authManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly ITableStyleRepository _tableStyleRepository;

        public LayerEditorController(IAuthManager authManager, IDatabaseManager databaseManager, ITableStyleRepository tableStyleRepository)
        {
            _authManager = authManager;
            _databaseManager = databaseManager;
            _tableStyleRepository = tableStyleRepository;
        }

        public class GetRequest
        {
            public int Id { get; set; }
            public string TableName { get; set; }
            public string AttributeName { get; set; }
            public string RelatedIdentities { get; set; }
        }

        public class GetResult
        {
            public IEnumerable<KeyValuePair<InputType, string>> InputTypes { get; set; }
            public SubmitRequest Form { get; set; }
        }

        public class SubmitRequest
        {
            public int Id { get; set; }
            public string TableName { get; set; }
            public string AttributeName { get; set; }
            public string RelatedIdentities { get; set; }
            public bool IsRapid { get; set; }
            public string RapidValues { get; set; }
            public int Taxis { get; set; }
            public string DisplayName { get; set; }
            public string HelpText { get; set; }
            public InputType InputType { get; set; }
            public string DefaultValue { get; set; }
            public bool Horizontal { get; set; }
            public List<InputStyleItem> Items { get; set; }
            public int Height { get; set; }
            public string CustomizeCode { get; set; }
            public bool Locked { get; set; }
            public bool Valid { get; set; }
        }
    }
}
