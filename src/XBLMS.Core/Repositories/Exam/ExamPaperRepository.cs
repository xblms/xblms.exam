using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperRepository : IExamPaperRepository
    {
        private readonly Repository<ExamPaper> _repository;

        public ExamPaperRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPaper>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }
        public async Task<int> InsertAsync(ExamPaper item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamPaper item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task<List<ExamPaper>> GetListAsync(string keyword)
        {
            var query = new Query();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(ExamPaper.Title), like)
                    .OrWhereLike(nameof(ExamPaper.Subject), like)
                );
            }
            query.OrderByDesc(nameof(ExamPaper.Id));
            return await _repository.GetAllAsync(query);
        }
        public async Task<(int total, List<ExamPaper> list)> GetListAsync(List<int> treeIds, string keyword, int pageIndex, int pageSize)
        {
            var query = new Query();

            if (treeIds != null && treeIds.Count > 0)
            {
                query.WhereIn(nameof(ExamPaper.TreeId), treeIds);
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(ExamPaper.Title), like)
                    .OrWhereLike(nameof(ExamPaper.Subject), like)
                );
            }
            query.OrderByDesc(nameof(ExamPaper.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<ExamPaper> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<bool> DeleteAsync(int Id)
        {
            var result = await _repository.DeleteAsync(Id);
            return result;
        }
        public async Task<int> GetCountAsync(List<int> treeIds)
        {
            return await _repository.CountAsync(Q.WhereIn(nameof(ExamPaper.TreeId), treeIds));
        }
        public async Task<int> MaxAsync()
        {
            var maxId = await _repository.MaxAsync(nameof(ExamPaper.Id));
            if (maxId.HasValue) return maxId.Value;
            return 0;
        }
        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount()
        {
            var count = await _repository.CountAsync(Q.WhereNullOrFalse(nameof(ExamPaper.Moni)));
            var lockedCount = await _repository.CountAsync(Q.WhereNullOrFalse(nameof(ExamPaper.Moni)).WhereTrue(nameof(ExamPaper.Locked)));
            var unLockedCount = await _repository.CountAsync(Q.WhereNullOrFalse(nameof(ExamPaper.Moni)).WhereNullOrFalse(nameof(ExamPaper.Locked)));
            return (count, 0, 0, lockedCount, unLockedCount);
        }
        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCountMoni()
        {
            var count = await _repository.CountAsync(Q.WhereTrue(nameof(ExamPaper.Moni)));
            var lockedCount = await _repository.CountAsync(Q.WhereTrue(nameof(ExamPaper.Moni)).WhereTrue(nameof(ExamPaper.Locked)));
            var unLockedCount = await _repository.CountAsync(Q.WhereTrue(nameof(ExamPaper.Moni)).WhereNullOrFalse(nameof(ExamPaper.Locked)));
            return (count, 0, 0, lockedCount, unLockedCount);
        }
    }
}
