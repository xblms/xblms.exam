using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperStartRepository : IExamPaperStartRepository
    {
        private readonly Repository<ExamPaperStart> _repository;
        private readonly string _cacheKey;

        public ExamPaperStartRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPaperStart>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(ExamPaperStart item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task UpdateAsync(ExamPaperStart item)
        {
            await _repository.UpdateAsync(item);
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task IncrementAsync(int id)
        {
            await _repository.IncrementAsync(nameof(ExamPaperStart.ExamTimeSeconds), Q.Where(nameof(ExamPaperStart.Id), id), 5);
        }

        public async Task ClearByUserAsync(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperStart.UserId), userId));
        }
        public async Task ClearByPaperAsync(int paperId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperStart.ExamPaperId), paperId));
        }

        public async Task ClearByPaperAndUserAsync(int paperId, int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperStart.ExamPaperId), paperId).Where(nameof(ExamPaperStart.UserId), userId));
        }
        public async Task<ExamPaperStart> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<ExamPaperStart> GetNoSubmitAsync(int paperId, int userId)
        {
            return await _repository.GetAsync(Q.
                WhereNullOrFalse(nameof(ExamPaperStart.IsSubmit)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId).
                Where(nameof(ExamPaperStart.UserId), userId));
        }
        public async Task<int> GetNoSubmitIdAsync(int paperId, int userId)
        {
            return await _repository.GetAsync<int>(Q.
                Select(nameof(ExamPaperStart.Id)).
                WhereNullOrFalse(nameof(ExamPaperStart.IsSubmit)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId).
                Where(nameof(ExamPaperStart.UserId), userId));
        }
        public async Task<int> CountAsync(int paperId, int userId)
        {
            return await _repository.CountAsync(Q.
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId).
                Where(nameof(ExamPaperStart.UserId), userId));
        }


        public async Task<List<ExamPaperStart>> GetListAsync(int paperId, int userId)
        {
            return await _repository.GetAllAsync(Q.
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                 WhereTrue(nameof(ExamPaperStart.IsMark)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId).
                Where(nameof(ExamPaperStart.UserId), userId).
                OrderByDesc(nameof(ExamPaperStart.Id)));
        }
        public async Task<(int total, List<ExamPaperStart> list)> GetListAsync(int userId, string dateFrom, string dateTo,string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                WhereTrue(nameof(ExamPaperStart.IsMark)).
                Where(nameof(ExamPaperStart.UserId), userId);

            if (!string.IsNullOrWhiteSpace(dateFrom))
            {
                query.Where(nameof(ExamPaperStart.EndDateTime), ">=", dateFrom);
            }
            if (!string.IsNullOrWhiteSpace(dateTo))
            {
                query.Where(nameof(ExamPaperStart.EndDateTime), "<=", dateTo);
            }
            if(!string.IsNullOrEmpty(keyWords))
            {
                var like = $"%{keyWords}%";
                query.WhereLike(nameof(ExamPaperStart.KeyWords), like);
            }

            var total = await _repository.CountAsync(query);

            query.OrderByDesc(nameof(ExamPaperStart.Id));
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<List<int>> GetPaperIdsAsync(int userId)
        {
            return await _repository.GetAllAsync<int>(Q.
                Select(nameof(ExamPaperStart.ExamPaperId)).
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                WhereTrue(nameof(ExamPaperStart.IsMark)).
                Where(nameof(ExamPaperStart.UserId), userId));
        }
        public async Task<int?> GetMaxScoreAsync(int userId,int paperId)
        {
            return await _repository.MaxAsync(nameof(ExamPaperStart.Score), Q.
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                WhereTrue(nameof(ExamPaperStart.IsMark)).
                Where(nameof(ExamPaperStart.UserId), userId).
                 Where(nameof(ExamPaperStart.ExamPaperId), paperId));
        }
        public async Task UpdateLockedAsync(int paperId, bool locked)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamPaperStart.Locked), locked).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId));
        }
        public async Task UpdateKeyWordsAsync(int paperId, string keyWords)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamPaperStart.KeyWords), keyWords).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId));
        }
    }
}
