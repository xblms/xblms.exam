using System.Threading.Tasks;
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
    }
}
