using Datory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Database
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class TablesController : ControllerBase
    {
        private const string Route = "settings/databaseTables";
        private readonly IDatabaseManager _databaseManager;
        private readonly IAuthManager _authManager;
        public class GetResult
        {
            public List<string> TableNames { get; set; }
        }

        public class PostRequest
        {
            public string TableName { get; set; }
        }

        public class PostResult
        {
            public List<TableColumn> TableColumns { get; set; }
            public int Count { get; set; }
        }
        public TablesController(IAuthManager authManager, IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
            _authManager= authManager;

        }

        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }
            return new GetResult
            {
                TableNames = await _databaseManager.GetTableNamesAsync()
            };
        }

        [HttpPost, Route(Route)]
        public async Task<ActionResult<PostResult>> GetTableColumnInfoList([FromBody] PostRequest request)
        {
            return new PostResult
            {
                TableColumns = await _databaseManager.GetTableColumnInfoListAsync(request.TableName, null),
                Count = _databaseManager.GetCount(request.TableName)
            };
        }
    }
}
