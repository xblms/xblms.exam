using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Database
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class QueryController : ControllerBase
    {
        private const string Route = "settings/databaseQuery";
        private readonly IAuthManager _authManager;
        private readonly IDatabaseManager _databaseManager;
        public class QueryRequest
        {
            public string Query { get; set; }
        }

        public class QueryResult
        {
            public List<dynamic> Results { get; set; }
            public List<string> Properties { get; set; }
            public int Count { get; set; }
        }
        public QueryController(IAuthManager authManager, IDatabaseManager databaseManager)
        {
            _authManager = authManager;
            _databaseManager = databaseManager;

        }

        [HttpPost, Route(Route)]
        public async Task<ActionResult<QueryResult>> Query([FromBody] QueryRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }
            var admin = await _authManager.GetAdminAsync();

            if (!StringUtils.StartsWithIgnoreCase(request.Query, "SELECT"))
            {
                return this.Error("请输入有效的查询SQL语句！");
            }
            try
            {
                var results = await _databaseManager.QueryAsync(request.Query);
                List<string> properties = null;
                var count = 0;
                if (results != null)
                {
                    count = results.Count;
                    if (count > 0)
                    {
                        var dataInfo = results.FirstOrDefault();
                        properties = _databaseManager.GetPropertyKeysForDynamic(dataInfo);
                    }
                }
                return new QueryResult
                {
                    Results = results,
                    Properties = properties,
                    Count = count
                };
            }
            catch(Exception ex)
            {
                return this.Error($"错误：{ex.Message}");
            }



        }
    }
}
