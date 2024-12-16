using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class AdministratorsInRolesRepository : IAdministratorsInRolesRepository
    {
        private readonly Repository<AdministratorsInRoles> _repository;

        public AdministratorsInRolesRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<AdministratorsInRoles>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<List<int>> GetRoleIdsForAdminAsync(int adminId)
        {
            var roleIds = await _repository.GetAllAsync<int>(Q
                .Select(nameof(AdministratorsInRoles.RoleId))
                .Where(nameof(AdministratorsInRoles.AdminId), adminId));
            return roleIds;
        }

        public async Task<List<int>> GetUserIdsInRoleAsync(int roleId)
        {
            var userIds = await _repository.GetAllAsync<int>(Q
                .Select(nameof(AdministratorsInRoles.AdminId))
                .Where(nameof(AdministratorsInRoles.RoleId), roleId));
            return userIds;
        }

        public async Task<IList<string>> GetUsersInRoleAsync(string roleName)
        {
            var userNames = await _repository.GetAllAsync<string>();
            return userNames;
        }

        public async Task DeleteUserAsync(int adminId)
        {
            await _repository.DeleteAsync(Q
                .Where(nameof(AdministratorsInRoles.AdminId), adminId));
        }

        public async Task InsertAsync(AdministratorsInRoles info)
        {
            await _repository.InsertAsync(info);
        }
        public async Task DeleteByRoleIdAsync(int roleId)
        {
            await _repository.DeleteAsync(Q
                .Where(nameof(AdministratorsInRoles.RoleId), roleId));
        }

    }
}
