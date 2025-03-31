using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class KnowlegesRepository : IKnowlegesRepository
    {
        private readonly Repository<Knowledges> _repository;

        public KnowlegesRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<Knowledges>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(Knowledges item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(Knowledges item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task<List<Knowledges>> GetNewListAsync()
        {
            return await _repository.GetAllAsync(Q.WhereNullOrFalse(nameof(Knowledges.Locked)).OrderByDesc(nameof(Knowledges.Id)).ForPage(1, 4));
        }
        public async Task<(int total, List<Knowledges> list)> GetListAsync(int userId, int treeId, bool isTreeWithChild, bool like, bool collect, string keyword, int pageIndex, int pageSize)
        {
            var query = Q.WhereNullOrFalse(nameof(Knowledges.Locked));
            if (treeId > 0)
            {
                if (isTreeWithChild)
                {
                    query.WhereLike(nameof(Knowledges.TreeParentPath), $"%'{treeId}'%");
                }
                else
                {
                    query.Where(nameof(Knowledges.TreeId), treeId);
                }
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var likeQ = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(Knowledges.Name), likeQ)
                );
            }
            if (like)
            {
                query.WhereLike(nameof(Knowledges.LikeUserIds), $"'{userId}'");
            }
            if (collect)
            {
                query.WhereLike(nameof(Knowledges.CollectUserIds), $"'{userId}'");

            }

            query.OrderByDesc(nameof(Knowledges.Likes), nameof(Knowledges.Collects), nameof(Knowledges.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int total, List<Knowledges> list)> GetListAsync(int treeId, bool isTreeWithChild, string keyword, int pageIndex, int pageSize)
        {
            var query = Q.NewQuery();
            if (treeId > 0)
            {
                if (isTreeWithChild)
                {
                    query.WhereLike(nameof(Knowledges.TreeParentPath), $"%'{treeId}'%");
                }
                else
                {
                    query.Where(nameof(Knowledges.TreeId), treeId);
                }
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(Knowledges.Name), like)
                );
            }

            query.OrderByDesc(nameof(Knowledges.Likes), nameof(Knowledges.Collects), nameof(Knowledges.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }
        public async Task<Knowledges> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<bool> DeleteAsync(int Id)
        {
            var result = await _repository.DeleteAsync(Id);
            return result;
        }
        public async Task<int> CountAsync(int treeId, bool withChild = true)
        {
            var query = new Query();
            if (treeId > 0)
            {
                if (withChild)
                {
                    query.WhereLike(nameof(Knowledges.TreeParentPath), $"%'{treeId}'%");
                }
                else
                {
                    query.Where(nameof(Knowledges.TreeId), treeId);
                }
            }
            return await _repository.CountAsync(query);
        }

        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount()
        {
            var count = await _repository.CountAsync();
            var lockedCount = await _repository.CountAsync(Q.WhereTrue(nameof(Knowledges.Locked)));
            var unLockedCount = await _repository.CountAsync(Q.WhereNullOrFalse(nameof(Knowledges.Locked)));
            return (count, 0, 0, lockedCount, unLockedCount);
        }
    }
}
