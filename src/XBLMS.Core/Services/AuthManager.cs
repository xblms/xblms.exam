using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class AuthManager : IAuthManager
    {
        private readonly IHttpContextAccessor _context;
        private readonly IAntiforgery _antiforgery;
        private readonly ClaimsPrincipal _principal;
        private readonly ICacheManager _cacheManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IDatabaseManager _databaseManager;

        public AuthManager(IHttpContextAccessor context, IAntiforgery antiforgery, ICacheManager cacheManager, ISettingsManager settingsManager, IDatabaseManager databaseManager)
        {
            _context = context;
            _antiforgery = antiforgery;
            _principal = context.HttpContext.User;
            _cacheManager = cacheManager;
            _settingsManager = settingsManager;
            _databaseManager = databaseManager;
        }

        private Administrator _admin;
        private User _user;

        public async Task InitAsync(User user)
        {
            if (user == null || user.Locked)
            {
                _user = null;
                return;
            }

            var token = await _context.HttpContext.GetTokenAsync("access_token");
            var cachedToken = _cacheManager.Get<string>(GetTokenCacheKey(user));
            if (!string.IsNullOrEmpty(cachedToken) && token != cachedToken)
            {
                _user = null;
                return;
            }

            _user = user;
        }

        public async Task<User> GetUserAsync()
        {
            if (!IsUser) return null;
            if (_user != null) return _user;

            _user = await _databaseManager.UserRepository.GetByUserNameAsync(UserName);

            await InitAsync(_user);

            return _user;
        }

        public async Task InitAsync(Administrator administrator)
        {
            if (administrator == null || administrator.Locked)
            {
                _admin = null;
                return;
            }

            var token = await _context.HttpContext.GetTokenAsync("access_token");
            var cachedToken = _cacheManager.Get<string>(GetTokenCacheKey(administrator));
            if (!string.IsNullOrEmpty(cachedToken) && token != cachedToken)
            {
                _admin = null;
                return;
            }

            _admin = administrator;
        }

        public async Task<Administrator> GetAdminAsync()
        {
            if (_admin != null) return _admin;

            if (IsAdmin)
            {
                _admin = await _databaseManager.AdministratorRepository.GetByUserNameAsync(AdminName);
                await InitAsync(_admin);
            }
            else if (IsUser)
            {
                var user = await GetUserAsync();
                if (user != null && !user.Locked)
                {
                    _admin = await _databaseManager.AdministratorRepository.GetByUserNameAsync("");
                }
            }

            return _admin;
        }

        public bool IsAdmin => _principal != null && _principal.IsInRole(Types.Roles.Administrator);

        public int AdminId => IsAdmin
            ? TranslateUtils.ToInt(_principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value)
            : 0;

        public string AdminName => IsAdmin ? _principal.Identity.Name : string.Empty;

        public bool IsUser => _principal != null && _principal.IsInRole(Types.Roles.User);

        public int UserId => IsUser
            ? TranslateUtils.ToInt(_principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value)
            : 0;

        public string UserName => IsUser ? _principal.Identity.Name : string.Empty;

        public bool IsApi => ApiToken != null;

        public string ApiToken
        {
            get
            {
                if (_context.HttpContext.Request.Query.TryGetValue("apiKey", out var queries))
                {
                    var token = queries.SingleOrDefault();
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        return token;
                    }
                }
                if (_context.HttpContext.Request.Headers.TryGetValue("X-XBLM-API-KEY", out var headers))
                {
                    var token = headers.SingleOrDefault();
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        return token;
                    }
                }

                return null;
            }
        }

        public string GetCSRFToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(_context.HttpContext);
            return tokens.RequestToken;
        }

        public string MenuId
        {
            get
            {
                if (_context.HttpContext.Request.Headers.TryGetValue("Permission", out var headers))
                {
                    var menuId = headers.SingleOrDefault();
                    if (!string.IsNullOrWhiteSpace(menuId))
                    {
                        return menuId;
                    }
                }

                return null;
            }
        }
    }
}
