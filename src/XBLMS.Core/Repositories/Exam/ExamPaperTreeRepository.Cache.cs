using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperTreeRepository
    {
        public async Task<List<ExamPaperTree>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<List<ExamPaperTree>> GetListAsync(AdminAuth auth)
        {
            var query = Q.OrderBy(nameof(ExamPaperTree.Id));
            query = GetQueryByAuth(query, auth);
            return await _repository.GetAllAsync(query);
        }
        public async Task<ExamPaperTree> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<List<int>> GetIdsAsync(int id)
        {
            return await _repository.GetAllAsync<int>(Q.
                 Select(nameof(ExamPaperTree.Id)).
                 WhereLike(nameof(ExamPaperTree.ParentPath), $"%'{id}'%"));
        }

        private async Task GetIdsAsync(List<int> ids, List<ExamPaperTree> all, int parentid)
        {
            var children = all.Where(x => x.ParentId == parentid).ToList();
            foreach (var child in children)
            {
                ids.Add(child.Id);
                await GetIdsAsync(ids, all, child.Id);
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

            var current = await _repository.GetAsync(parentId);

            if (current.ParentId > 0)
            {
                await GetParentIdsAsync(ids, current.ParentId);
            }
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
