using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class AuthManager
    {

        public async Task AddAdminLogAsync(string action, string summary)
        {
            var admin = await GetAdminAsync();
            if (admin != null)
            {
                var ipAddress = PageUtils.GetIpAddress(_context.HttpContext.Request);
                await _databaseManager.LogRepository.AddAdminLogAsync(admin, ipAddress, action, summary);
            }
        }

        public async Task AddAdminLogAsync(string action)
        {
            var admin = await GetAdminAsync();
            if (admin != null)
            {
                var ipAddress = PageUtils.GetIpAddress(_context.HttpContext.Request);
                await _databaseManager.LogRepository.AddAdminLogAsync(admin, ipAddress, action);
            }
        }

        public async Task AddUserLogAsync(string action, string summary)
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                var ipAddress = PageUtils.GetIpAddress(_context.HttpContext.Request);
                await _databaseManager.LogRepository.AddUserLogAsync(user, ipAddress, action, summary);
            }
        }

        public async Task AddUserLogAsync(string action)
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                var ipAddress = PageUtils.GetIpAddress(_context.HttpContext.Request);
                await _databaseManager.LogRepository.AddUserLogAsync(user, ipAddress, action);
            }
        }


        public async Task AddStatLogAsync(StatType statType, string statTypeStr, int objectId = 0, string objectName = "", object entity = null)
        {
            var admin = await GetAdminAsync();
            if (admin != null)
            {
                var ipAddress = PageUtils.GetIpAddress(_context.HttpContext.Request);
                var entityStr = string.Empty;
                if (entity != null)
                {
                    entityStr = TranslateUtils.JsonSerialize(entity);
                }
                await _databaseManager.StatLogRepository.InsertAsync(statType, statTypeStr, ipAddress, admin.Id, objectId, objectName, entityStr);
            }
        }

        public async Task AddStatCount(StatType statType)
        {
            var admin = await GetAdminAsync();
            await _databaseManager.StatRepository.AddCountAsync(statType, admin.Id);
        }
    }
}
