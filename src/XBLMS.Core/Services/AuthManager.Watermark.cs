using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Utils;

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
