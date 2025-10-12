using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly Repository<Role> _repository;

        public RoleRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<Role>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<Role> GetRoleAsync(int roleId)
        {
            return await _repository.GetAsync(roleId);
        }

        public async Task<List<Role>> GetRolesAsync(AdminAuth auth)
        {
            var query = Q.OrderByDesc(nameof(Role.Id));
            query = GetQueryByAuth(query, auth);
            return await _repository.GetAllAsync(query);
        }


        public async Task<int> InsertRoleAsync(Role role)
        {
            if (IsPredefinedRole(role.RoleName)) return 0;

            return await _repository.InsertAsync(role);
        }

        public async Task UpdateRoleAsync(Role role)
        {
            await _repository.UpdateAsync(role);
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            return await _repository.DeleteAsync(roleId);
        }

        public async Task<bool> IsRoleExistsAsync(string roleName)
        {
            return await _repository.ExistsAsync(Q.Where(nameof(Role.RoleName), roleName));
        }

        public bool IsPredefinedRole(string roleName)
        {
            var roles = ListUtils.GetEnums<PredefinedRole>().Select(x => x.GetValue()).ToList();
            return ListUtils.ContainsIgnoreCase(roles, roleName);
        }

        public async Task<List<Role>> GetRolesAsync(AdminAuth auth, string keyWords)
        {
            var query = Q.NewQuery();

            query = GetQueryByAuth(query, auth);

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.Where(q =>
                {
                    q.WhereLike(nameof(Role.RoleName), $"%{keyWords}%")
                    .OrWhereLike(nameof(Role.Description), $"%{keyWords}%");
                    return q;
                });
            }

            return await _repository.GetAllAsync(query.OrderByDesc(nameof(Role.Id)));
        }
        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(Role.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(Role.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(Role.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
