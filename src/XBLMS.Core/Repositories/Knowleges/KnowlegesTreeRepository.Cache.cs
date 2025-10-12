using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class KnowlegesTreeRepository
    {
        public async Task<List<KnowledgesTree>> GetListAsync(AdminAuth auth)
        {
            var query = Q.OrderBy(nameof(KnowledgesTree.Id));
            query = GetQueryByAuth(query, auth);
            return await _repository.GetAllAsync(query);
        }

        public async Task<KnowledgesTree> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
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

        public async Task<List<int>> GetIdsAsync(int id)
        {
            return await _repository.GetAllAsync<int>(Q.
                Select(nameof(KnowledgesTree.Id)).
                WhereLike(nameof(KnowledgesTree.ParentPath), $"%'{id}'%"));
        }


        private async Task GetParentIdsAsync(List<int> ids, int parentId)
        {
            ids.Add(parentId);

            var current = await _repository.GetAsync(parentId);

            if (current.ParentId > 0)
            {
                await GetParentIdsAsync(ids, current.ParentId);
            }
        }

        public async Task<List<KnowledgesTree>> GetChildAsync(int parentId)
        {
            var query = Q.Where(nameof(KnowledgesTree.ParentId), parentId);
            return await _repository.GetAllAsync(query);
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(KnowledgesTree.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(KnowledgesTree.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(KnowledgesTree.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
