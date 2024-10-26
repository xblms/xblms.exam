using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin
{
    [OpenApiIgnore]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ClearDatabaseController : ControllerBase
    {
        public const string Route = "clearDatabase";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly IConfigRepository _configRepository;

        public ClearDatabaseController(ISettingsManager settingsManager, IAuthManager authManager, IDatabaseManager databaseManager, IConfigRepository configRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _databaseManager = databaseManager;
            _configRepository = configRepository;
        }


        public class SubmitRequest
        {
            public string SecurityKey { get; set; }
        }


        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] SubmitRequest request)
        {
            var config = await _configRepository.GetAsync();

            if (config.DatabaseVersion == _settingsManager.Version)
            {
                if (_settingsManager.SecurityKey != request.SecurityKey)
                {
                    return this.Error("SecurityKey 输入错误！");
                }
            }

            await _databaseManager.ClearDatabaseAsync();

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
