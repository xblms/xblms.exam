using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Utilities
{
    public partial class UtilitiesParametersController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var config = await _configRepository.GetAsync();

            var environments = new List<KeyValuePair<string, string>>
            {

            };

            if (!_settingsManager.IsSafeMode)
            {
                environments.Add(new KeyValuePair<string, string>("运行环境", _settingsManager.Containerized ? "容器" : "主机"));
                environments.Add(new KeyValuePair<string, string>(".NET Core 版本", _settingsManager.FrameworkDescription));
                environments.Add(new KeyValuePair<string, string>("OS 版本", _settingsManager.OSDescription));
                environments.Add(new KeyValuePair<string, string>("CPU Cores", _settingsManager.CPUCores.ToString()));
                environments.Add(new KeyValuePair<string, string>("系统主机名", Dns.GetHostName().ToUpper()));
            }
            else
            {
                environments.Add(new KeyValuePair<string, string>("运行环境", "******"));
                environments.Add(new KeyValuePair<string, string>(".NET Core 版本", "******"));
                environments.Add(new KeyValuePair<string, string>("OS 版本", "******"));
                environments.Add(new KeyValuePair<string, string>("CPU Cores", "******"));
                environments.Add(new KeyValuePair<string, string>("系统主机名", "******"));
            }

            var settings = new List<KeyValuePair<string, string>>();

            if (!_settingsManager.IsSafeMode)
            {
                settings.Add(new KeyValuePair<string, string>("系统根目录地址", _settingsManager.ContentRootPath));
                settings.Add(new KeyValuePair<string, string>("站点根目录地址", _settingsManager.WebRootPath));
                settings.Add(new KeyValuePair<string, string>("数据库类型", _settingsManager.Database.DatabaseType.GetDisplayName()));
                settings.Add(new KeyValuePair<string, string>("数据库名称", _databaseManager.GetDatabaseNameFormConnectionString(_settingsManager.Database.ConnectionString)));
                settings.Add(new KeyValuePair<string, string>("缓存类型", string.IsNullOrEmpty(_settingsManager.Redis.ConnectionString) ? "Memory" : "Redis"));
            }
            else
            {
                settings.Add(new KeyValuePair<string, string>("系统根目录地址", "******"));
                settings.Add(new KeyValuePair<string, string>("站点根目录地址", "******"));
                settings.Add(new KeyValuePair<string, string>("数据库类型", "******"));
                settings.Add(new KeyValuePair<string, string>("数据库名称", "******"));
                settings.Add(new KeyValuePair<string, string>("缓存类型", "******"));
            }

            return new GetResult
            {
                Environments = environments,
                Settings = settings
            };
        }
    }
}
