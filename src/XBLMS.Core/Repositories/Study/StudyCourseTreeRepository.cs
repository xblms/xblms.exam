using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class StudyCourseTreeRepository : IStudyCourseTreeRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyCourseTree> _repository;

        public StudyCourseTreeRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyCourseTree>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(StudyCourseTree item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(StudyCourseTree item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(Q.WhereLike(nameof(StudyCourseTree.ParentPath), $"%'{id}'%")) > 0;
        }


        public async Task<List<StudyCourseTree>> GetListAsync(AdminAuth auth)
        {
            var query = Q.OrderBy(nameof(StudyCourseTree.Id));
            query = GetQueryByAuth(query, auth);
            return await _repository.GetAllAsync(query);
        }
        public async Task<StudyCourseTree> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }


        public async Task<List<int>> GetIdsAsync(int id)
        {
            return await _repository.GetAllAsync<int>(Q.
                 Select(nameof(StudyCourseTree.Id)).
                 WhereLike(nameof(StudyCourseTree.ParentPath), $"%'{id}'%"));
        }

        private async Task GetIdsAsync(List<int> ids, List<StudyCourseTree> all, int parentid)
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
                query.Where(nameof(StudyCourseTree.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(StudyCourseTree.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(StudyCourseTree.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
