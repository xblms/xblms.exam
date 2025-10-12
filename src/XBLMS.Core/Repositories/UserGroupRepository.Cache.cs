using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class UserGroupRepository
    {
        public async Task<bool> ExistsAsync(string name, int companyId)
        {
            return await _repository.ExistsAsync(Q.Where(nameof(UserGroup.GroupName), name).Where(nameof(UserGroup.CompanyId), companyId));
        }
        public async Task<UserGroup> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<List<UserGroup>> GetListAsync(AdminAuth auth, bool withoutLocked = false)
        {
            var query = Q.NewQuery();
            if (withoutLocked)
            {
                query.WhereNullOrFalse(nameof(UserGroup.Locked));
            }
            query = GetQueryByAuth(query, auth);
            var list = await _repository.GetAllAsync(query);
            return list;
        }
        public async Task<List<UserGroup>> GetListAsync(AdminAuth auth, string keyWords)
        {
            var query = Q.NewQuery();
            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(UserGroup.GroupName), $"%{keyWords}%");
            }
            query = GetQueryByAuth(query, auth);
            query.OrderByDesc(nameof(UserGroup.Id));
            var list = await _repository.GetAllAsync(query);
            return list;
        }
        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(UserGroup.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(UserGroup.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(UserGroup.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
