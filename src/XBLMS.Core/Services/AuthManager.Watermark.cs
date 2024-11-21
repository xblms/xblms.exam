using System;
using System.Threading.Tasks;

namespace XBLMS.Core.Services
{
    public partial class AuthManager
    {
        public async Task<string> GetWatermark()
        {
            var user = await GetUserAsync();
            return $"{user.UserName}-{user.DisplayName}-{DateTime.Now.ToString("yyyyMMddhhmmss")}";
        }
    }
}
