using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class OrganDepartmentRepository
    {
        private string GetCacheKey(int id)
        {
            return CacheUtils.GetEntityKey(TableName, "id", id.ToString());
        }
        public async Task<bool> HasChildren(int parentId, int companyId)
        {
            return await _repository.ExistsAsync(Q.Where(nameof(OrganDepartment.CompanyId), companyId).Where(nameof(OrganDepartment.ParentId), parentId));
        }
        public async Task<List<OrganDepartment>> GetListAsync(int parentId, int companyId)
        {
            return await _repository.GetAllAsync(Q.Where(nameof(OrganDepartment.CompanyId), companyId).Where(nameof(OrganDepartment.ParentId), parentId).OrderBy(nameof(OrganDepartment.Id)));
        }
        public async Task<List<OrganDepartment>> GetListAsync(string keyWords)
        {
            return await _repository.GetAllAsync(Q.
                WhereLike(nameof(OrganDepartment.Name), $"%{keyWords}%").
                OrderBy(nameof(OrganDepartment.Id)));

        }
        public async Task<List<OrganDepartment>> GetListAsync()
        {
            return await GetAllAsync();
        }

        public async Task<List<OrganDepartment>> GetAllAsync()
        {
            return await _repository.GetAllAsync(Q
                .OrderBy(nameof(OrganDepartment.Id))
            );
        }
        public async Task<OrganDepartment> GetAsync(int companyId, string name)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(OrganDepartment.CompanyId), companyId).
                Where(nameof(OrganDepartment.Name), name));
        }
        public async Task<OrganDepartment> GetAsync(int id)
        {
            return await _repository.GetAsync(id, Q.CachingGet(GetCacheKey(id)));
        }
        public async Task<List<int>> GetIdsAsync(int id)
        {
            var ids = new List<int>
            {
                id
            };
            await GetIdsAsync(ids, id);
            return ids;
        }

        private async Task GetIdsAsync(List<int> ids, int parentid)
        {
            var childrenIds = await _repository.GetAllAsync<int>(Q.Select(nameof(OrganDepartment.Id)).Where(nameof(OrganDepartment.ParentId), parentid));
            foreach (var id in childrenIds)
            {
                ids.Add(id);
                await GetIdsAsync(ids, id);
            }
        }

        public async Task<List<string>> GetParentPathAsync(int id)
        {
            var current = await _repository.GetAsync(id);
            var ids = new List<int> { id };

            if (current.ParentId > 0)
            {
                await GetParentIdsAsync(ids, current.ParentId);
            }

            var path = new List<string>();
            for (var i = ids.Count - 1; i >= 0; i--)
            {
                path.Add($"'{ids[i]}'");
            }

            return path;
        }
        private async Task GetParentIdsAsync(List<int> ids, int parentId)
        {
            ids.Add(parentId);

            var current = await GetAsync(parentId);

            if (current.ParentId > 0)
            {
                await GetParentIdsAsync(ids, current.ParentId);
            }
        }

        public async Task<(List<string> path, List<string> names)> GetParentPathAndNamesAsync(int id)
        {
            var current = await GetAsync(id);
            var ids = new List<int> { id };
            var pathNames = new List<string> { current.Name };

            if (current.ParentId > 0)
            {
                await GetParentIdsAndNamesAsync(ids, pathNames, current.ParentId);
            }

            var path = new List<string>();
            var names = new List<string>();
            for (var i = ids.Count - 1; i >= 0; i--)
            {
                path.Add($"'{ids[i]}'");
            }
            for (var i = pathNames.Count - 1; i >= 0; i--)
            {
                names.Add(pathNames[i]);
            }
            return (path, names);
        }
        private async Task GetParentIdsAndNamesAsync(List<int> ids, List<string> names, int parentId)
        {
            ids.Add(parentId);

            var current = await GetAsync(parentId);
            names.Add(current.Name);

            if (current.ParentId > 0)
            {
                await GetParentIdsAndNamesAsync(ids, names, current.ParentId);
            }
        }

        public async Task<List<OrganDepartment>> GetListAsync(AdminAuth auth, string keywords)
        {
            var query = Q.
                WhereLike(nameof(OrganDepartment.Name), $"%{keywords}%").
                OrderBy(nameof(OrganDepartment.Id));

            query = GetQueryByAuth(query, auth);

            return await _repository.GetAllAsync(query);
        }

        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var query = Q.NewQuery();
            query = GetQueryByAuth(query, auth);
            var count = await _repository.CountAsync(query);
            return (count, 0, 0, 0, count);
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(OrganDepartment.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(OrganDepartment.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(OrganDepartment.Id), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
