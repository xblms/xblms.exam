using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class ExamTmTreeRepository
    {
        public async Task<List<ExamTmTree>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<List<ExamTmTree>> GetListAsync(AdminAuth auth)
        {
            var query = Q.OrderBy(nameof(ExamTmTree.Id));
            query = GetQueryByAuth(query, auth);
            return await _repository.GetAllAsync(query);
        }
        public async Task<ExamTmTree> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<List<int>> GetIdsAsync(int id)
        {
            return await _repository.GetAllAsync<int>(Q.
                 Select(nameof(ExamTmTree.Id)).
                 WhereLike(nameof(ExamTmTree.ParentPath), $"%'{id}'%"));
        }

        public async Task<string> GetPathNamesAsync(int id)
        {
            var result = new List<ExamTmTree>();
            var info = await GetAsync(id);
            if (info != null)
            {
                result.Add(info);
                await GetPathNamesAsync(result, info.ParentId);
            }
            result = result.OrderBy(d => d.Id).ToList();
            var names = new List<string>();
            if(result.Count > 0)
            {
                foreach (var item in result)
                {
                    names.Add(item.Name);
                }

            }
            return ListUtils.ToString(names, "/"); ;
        }
        public async Task GetPathNamesAsync(List<ExamTmTree> names, int parentId)
        {
            if (parentId > 0)
            {
                var info = await GetAsync(parentId);
                names.Add(info);
                await GetPathNamesAsync(names, info.ParentId);
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
                query.Where(nameof(ExamTmTree.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(ExamTmTree.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(ExamTmTree.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }

    }
}
