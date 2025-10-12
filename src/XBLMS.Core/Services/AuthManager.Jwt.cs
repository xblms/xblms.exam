using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class AuthManager
    {
        private ClaimsIdentity GetAdministratorIdentity(Administrator administrator, bool isPersistent)
        {
            return new ClaimsIdentity(new[]
            {
                new Claim(Types.Claims.UserId, administrator.Id.ToString()),
                new Claim(Types.Claims.UserName, administrator.UserName),
                new Claim(Types.Claims.Role, Types.Roles.Administrator),
                new Claim(Types.Claims.IsPersistent, isPersistent.ToString())
            });
        }

        private static string GetTokenCacheKey(Administrator admin)
        {
            return $"admin:{admin.Id}:token";
        }

        private static string GetTokenCacheKey(User user)
        {
            return $"user:{user.Id}:token";
        }

        public string AuthenticateAdministrator(Administrator administrator, bool isPersistent)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = StringUtils.GetSecurityKeyBytes(_settingsManager.SecurityKey);
            SecurityTokenDescriptor tokenDescriptor;
            var identity = GetAdministratorIdentity(administrator, isPersistent);

            if (isPersistent)
            {
                tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = identity,
                    Expires = DateTime.Now.AddDays(Constants.AccessTokenExpireDays),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
            }
            else
            {
                tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = identity,
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);

            _cacheManager.AddOrUpdate(GetTokenCacheKey(administrator), tokenString);

            return tokenString;
        }


        private ClaimsIdentity GetUserIdentity(User user, bool isPersistent)
        {
            return new ClaimsIdentity(new[]
            {
                new Claim(Types.Claims.UserId, user.Id.ToString()),
                new Claim(Types.Claims.UserName, user.UserName),
                new Claim(Types.Claims.Role, Types.Roles.User),
                new Claim(Types.Claims.IsPersistent, isPersistent.ToString())
            });
        }

        public string AuthenticateUser(User user, bool isPersistent)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = StringUtils.GetSecurityKeyBytes(_settingsManager.SecurityKey);
            SecurityTokenDescriptor tokenDescriptor;
            var identity = GetUserIdentity(user, isPersistent);

            if (isPersistent)
            {
                tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = identity,
                    Expires = DateTime.Now.AddDays(Constants.AccessTokenExpireDays),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
            }
            else
            {
                tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = identity,
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);

            _cacheManager.AddOrUpdate(GetTokenCacheKey(user), tokenString);

            return tokenString;
        }
    }
}
