using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Datory;
using Datory.Annotations;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Newtonsoft.Json.Converters;
using NPOI.POIFS.Properties;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class OrganManager
    {
        public async Task<Administrator> GetAdministrator(int adminId)
        {
            var admin = await _administratorRepository.GetByUserIdAsync(adminId);

            admin.Remove("confirmPassword");

            var roleNames = await _administratorRepository.GetRoleNames(admin.Id);
            var organNames = await GetOrganName(admin.DutyId, admin.DepartmentId, admin.CompanyId);
            admin.Set("RoleNames", roleNames);
            admin.Set("OrganNames", organNames);
            admin.Set("AuthName", GetAdminAuthName(admin));

            return admin;
        }
        public async Task<User> GetUser(int userId)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);

            user.Remove("confirmPassword");

            var organNames = await GetOrganName(user.DutyId, user.DepartmentId, user.CompanyId);
            user.Set("OrganNames", organNames);
            user.KeyWordsAdmin = await GetUserKeyWords(user);
            return user;
        }
        public async Task GetUser(User user)
        {
            user.Remove("confirmPassword");
            var organNames = await GetOrganName(user.DutyId, user.DepartmentId, user.CompanyId);
            user.Set("OrganNames", organNames);
        }
        private static string GetAdminAuthName(Administrator admin)
        {
            return admin.Auth.GetDisplayName(); 
        }
        public async Task<string> GetUserKeyWords(int userId)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            var organNames = await GetOrganName(user.DutyId, user.DepartmentId, user.CompanyId);

            return $"{user.UserName}-{user.DisplayName}-{organNames}";
        }
        public async Task<string> GetUserKeyWords(User user)
        {
            var organNames = await GetOrganName(user.DutyId, user.DepartmentId, user.CompanyId);

            return $"{user.UserName}-{user.DisplayName}-{organNames}";
        }
    }
}
