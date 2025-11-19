using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

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
        public async Task IncrementAsync(int id)
        {
            await _repository.IncrementAsync(nameof(ExamPaperStart.ExamTimeSeconds), Q.Where(nameof(ExamPaperStart.Id), id), 5);
        }
        public async Task ClearByPaperAsync(int paperId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperStart.ExamPaperId), paperId));
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task DeleteByUserId(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperStart.UserId), userId));
        }
        public async Task ClearByPaperAndUserAsync(int paperId, int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperStart.ExamPaperId), paperId).Where(nameof(ExamPaperStart.UserId), userId));
        }
        public async Task<ExamPaperStart> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<ExamPaperStart> GetNoSubmitAsync(int planId, int courseId, int paperId, int userId)
        {
            return await _repository.GetAsync(Q.
                WhereNullOrFalse(nameof(ExamPaperStart.IsSubmit)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId).
                Where(nameof(ExamPaperStart.PlanId), planId).
                Where(nameof(ExamPaperStart.CourseId), courseId).
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
        public async Task<int> GetNoSubmitIdAsync(int paperId, int userId, int planId, int courseId)
        {
            return await _repository.GetAsync<int>(Q.
                Select(nameof(ExamPaperStart.Id)).
                WhereNullOrFalse(nameof(ExamPaperStart.IsSubmit)).
                Where(nameof(ExamPaperStart.PlanId), planId).
                Where(nameof(ExamPaperStart.CourseId), courseId).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId).
                Where(nameof(ExamPaperStart.UserId), userId));
        }
        public async Task<List<ExamPaperStart>> GetNoSubmitListAsync(int paperId, int userId)
        {
            return await _repository.GetAllAsync(Q.
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
        public async Task<int> CountAsync(int paperId, int userId, int planId, int courseId)
        {
            return await _repository.CountAsync(Q.
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                Where(nameof(ExamPaperStart.PlanId), planId).
                Where(nameof(ExamPaperStart.CourseId), courseId).
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
        public async Task<List<ExamPaperStart>> GetListAsync(int paperId, int userId, int planId, int courseId)
        {
            return await _repository.GetAllAsync(Q.
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                 WhereTrue(nameof(ExamPaperStart.IsMark)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId).
                Where(nameof(ExamPaperStart.UserId), userId).
                Where(nameof(ExamPaperStart.PlanId), planId).
                Where(nameof(ExamPaperStart.CourseId), courseId).
                OrderByDesc(nameof(ExamPaperStart.Id)));
        }
        public async Task<(int total, List<ExamPaperStart> list)> GetListAsync(int userId, string dateFrom, string dateTo, string keyWords, int pageIndex, int pageSize)
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
            if (!string.IsNullOrEmpty(keyWords))
            {
                var like = $"%{keyWords}%";
                query.WhereLike(nameof(ExamPaperStart.KeyWords), like);
            }

            var total = await _repository.CountAsync(query);

            query.OrderByDesc(nameof(ExamPaperStart.Id));
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int total, List<ExamPaperStart> list)> GetListByAdminAsync(int paperId, int planId, int courseId, string dateFrom, string dateTo, string keyWords, int pageIndex, int pageSize, bool isMark = true)
        {
            var query = Q.
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId);

            if (planId > 0 || courseId > 0)
            {
                query.Where(nameof(ExamPaperStart.PlanId), planId).Where(nameof(ExamPaperStart.CourseId), courseId);
            }

            if (isMark)
            {
                query.WhereTrue(nameof(ExamPaperStart.IsMark));
            }
            else
            {
                query.WhereNullOrFalse(nameof(ExamPaperStart.IsMark));
            }

            if (!string.IsNullOrWhiteSpace(dateFrom))
            {
                query.Where(nameof(ExamPaperStart.EndDateTime), ">=", dateFrom);
            }
            if (!string.IsNullOrWhiteSpace(dateTo))
            {
                query.Where(nameof(ExamPaperStart.EndDateTime), "<=", dateTo);
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                var like = $"%{keyWords}%";
                query.WhereLike(nameof(ExamPaperStart.KeyWordsAdmin), like);
            }

            var total = await _repository.CountAsync(query);

            query.OrderByDesc(nameof(ExamPaperStart.Id));
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int total, List<ExamPaperStart> list)> GetListByMarkerAsync(int markerId, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                WhereNullOrFalse(nameof(ExamPaperStart.Locked)).
                Where(nameof(ExamPaperStart.MarkTeacherId), markerId);


            if (!string.IsNullOrEmpty(keyWords))
            {

                var like = $"%{keyWords}%";
                query.WhereLike(nameof(ExamPaperStart.KeyWords), like);
            }

            var total = await _repository.CountAsync(query);

            query.OrderByDesc(nameof(ExamPaperStart.Id));
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
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

        public async Task<decimal> GetMaxScoreAsync(int userId, int paperId, int planId, int courseId)
        {
            var maxItem = await _repository.GetAsync(Q.
                 WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                 WhereTrue(nameof(ExamPaperStart.IsMark)).
                 Where(nameof(ExamPaperStart.ExamPaperId), paperId).
                 Where(nameof(ExamPaperStart.UserId), userId).
                 Where(nameof(ExamPaperStart.PlanId), planId).
                 Where(nameof(ExamPaperStart.CourseId), courseId).
                 OrderByDesc(nameof(ExamPaperStart.Score)).Limit(1));
            if (maxItem != null)
            {
                return maxItem.Score;
            }
            return 0;
        }

        public async Task<decimal> GetMaxScoreAsync(int paperId)
        {
            var maxItem = await _repository.GetAsync(Q.
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                WhereTrue(nameof(ExamPaperStart.IsMark)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId).
                OrderByDesc(nameof(ExamPaperStart.Score)).Limit(1));
            if (maxItem != null)
            {
                return maxItem.Score;
            }
            return 0;
        }
        public async Task<decimal> GetMinScoreAsync(int paperId)
        {
            var minItem = await _repository.GetAsync(Q.
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                WhereTrue(nameof(ExamPaperStart.IsMark)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId).
                OrderBy(nameof(ExamPaperStart.Score)).Limit(1));
            if (minItem != null)
            {
                return minItem.Score;
            }
            return 0;
        }
        public async Task<int> CountAsync(int paperId)
        {
            return await _repository.CountAsync(Q.
                WhereTrue(nameof(ExamPaperStart.IsMark)).
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId));
        }
        public async Task<int> CountDistinctAsync(int paperId)
        {
            var userIds = await _repository.GetAllAsync<int>(Q.
               Select(nameof(ExamPaperStart.UserId)).
               WhereTrue(nameof(ExamPaperStart.IsMark)).
               WhereTrue(nameof(ExamPaperStart.IsSubmit)).
               Where(nameof(ExamPaperStart.ExamPaperId), paperId));

            if (userIds != null && userIds.Count > 0)
            {
                return userIds.Distinct().Count();
            }
            return 0;
        }
        public async Task<decimal> SumScoreAsync(int paperId)
        {
            var listScore = await _repository.GetAllAsync<decimal>(Q.
                Select(nameof(ExamPaperStart.Score)).
                WhereTrue(nameof(ExamPaperStart.IsMark)).
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId));
            if (listScore != null && listScore.Count > 0)
            {
                return listScore.Sum();
            }
            return 0;
        }

        public async Task<int> CountByPassAsync(int paperId, int passScore)
        {
            return await _repository.CountAsync(Q.
                Where(nameof(ExamPaperStart.Score), ">=", passScore).
                WhereTrue(nameof(ExamPaperStart.IsMark)).
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId));
        }
        public async Task<int> CountByPassDistinctAsync(int paperId, int passScore)
        {
            var userIds = await _repository.GetAllAsync<int>(Q.
            Select(nameof(ExamPaperStart.UserId)).
            Where(nameof(ExamPaperStart.Score), ">=", passScore).
            WhereTrue(nameof(ExamPaperStart.IsMark)).
            WhereTrue(nameof(ExamPaperStart.IsSubmit)).
            Where(nameof(ExamPaperStart.ExamPaperId), paperId));

            if (userIds != null && userIds.Count > 0)
            {
                return userIds.Distinct().Count();
            }
            return 0;

        }
        public async Task<int> CountByMarkAsync(int paperId)
        {
            return await _repository.CountAsync(Q.
                WhereNullOrFalse(nameof(ExamPaperStart.IsMark)).
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                Where(nameof(ExamPaperStart.ExamPaperId), paperId));
        }
    }
}
