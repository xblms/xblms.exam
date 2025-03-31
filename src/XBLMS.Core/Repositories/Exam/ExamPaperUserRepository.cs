using Datory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperUserRepository : IExamPaperUserRepository
    {
        private readonly Repository<ExamPaperUser> _repository;
        private readonly string _cacheKey;

        public ExamPaperUserRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPaperUser>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<int> InsertAsync(ExamPaperUser item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task DeleteByUserId(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperUser.UserId), userId));
        }
        public async Task ClearByPaperAsync(int paperId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperUser.ExamPaperId), paperId));
        }
        public async Task<bool> ExistsAsync(int paperId, int userId)
        {
            return await _repository.ExistsAsync(Q.Where(nameof(ExamPaperUser.ExamPaperId), paperId).Where(nameof(ExamPaperUser.UserId), userId));
        }

        public async Task<ExamPaperUser> GetAsync(int paperId, int userId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(ExamPaperUser.ExamPaperId), paperId).
                Where(nameof(ExamPaperUser.UserId), userId));
        }
        public async Task<ExamPaperUser> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<List<int>> GetPaperIdsByUser(int userId)
        {
            var query = Q.
                Select(nameof(ExamPaperUser.ExamPaperId)).
                WhereNullOrFalse(nameof(ExamPaperUser.Locked)).
                Where(nameof(ExamPaperUser.ExamEndDateTime), ">",DateUtils.ToString(DateTime.Now)).
                Where(nameof(ExamPaperUser.UserId), userId).
                OrderByDesc(nameof(ExamPaperUser.Id));

            return await _repository.GetAllAsync<int>(query);
        }

        public async Task<(int total, List<ExamPaperUser> list)> GetListAsync(int paperId, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.
                Where(nameof(ExamPaperUser.ExamPaperId), paperId);

            if (!string.IsNullOrEmpty(keyWords))
            {
                var like = $"%{keyWords}%";
                query.WhereLike(nameof(ExamPaperUser.KeyWordsAdmin), like);
            }

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));

            return (total, list);
        }

        public async Task<(int total, List<ExamPaperUser> list)> GetListAsync(int userId, bool isMoni, bool isApp, string date, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.
                WhereNullOrFalse(nameof(ExamPaperUser.Locked)).
                Where(nameof(ExamPaperUser.UserId), userId);
            if (isApp)
            {
                query.WhereNullOrFalse(nameof(ExamPaperUser.LockedApp));
            }
            if (isMoni)
            {
                query.WhereTrue(nameof(ExamPaperUser.Moni));
            }
            else
            {
                query.WhereNullOrFalse(nameof(ExamPaperUser.Moni));
            }
            if (!string.IsNullOrWhiteSpace(date))
            {
                var dateFrom = DateTime.Now;
                var dateTo = DateTime.Now;

                if (date == "three")
                {
                    dateTo = dateTo.AddDays(2);
                }
                if (date == "week")
                {
                    dateTo = dateTo.AddDays(6);
                }
                if (date == "month")
                {
                    dateTo = dateTo.AddMonths(1);
                }
                if (date == "year")
                {
                    dateTo = dateTo.AddYears(1);
                }
                var dateFromStr = dateFrom.ToString("yyyy-MM-dd 00:00:00");
                var dateToStr = dateTo.ToString("yyyy-MM-dd 23:59:59");

                query.Where(nameof(ExamPaperUser.ExamBeginDateTime), ">=", DateUtils.ToString(dateFromStr));
                query.Where(nameof(ExamPaperUser.ExamBeginDateTime), "<=", DateUtils.ToString(dateToStr));
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                var like = $"%{keyWords}%";
                query.WhereLike(nameof(ExamPaperUser.KeyWords), like);
            }
            var count = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.OrderByDesc(nameof(ExamPaperUser.Id)).ForPage(pageIndex, pageSize));

            return (count, list);
        }

        public async Task IncrementAsync(int id)
        {
            await _repository.IncrementAsync(nameof(ExamPaperUser.ExamTimes), Q.Where(nameof(ExamPaperUser.Id), id));
        }
        public async Task DecrementAsync(int id)
        {
            await _repository.DecrementAsync(nameof(ExamPaperUser.ExamTimes), Q.
                Where(nameof(ExamPaperUser.ExamTimes), ">", 0).
                Where(nameof(ExamPaperUser.Id), id));
        }
        public async Task UpdateExamDateTimeByIdAsync(int id, DateTime beginDateTime, DateTime endDateTime)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamPaperUser.ExamBeginDateTime), beginDateTime).
                Set(nameof(ExamPaperUser.ExamEndDateTime), endDateTime).
                Where(nameof(ExamPaperUser.Id), id));
        }
        public async Task UpdateExamDateTimeAsync(int paperId, DateTime beginDateTime, DateTime endDateTime)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamPaperUser.ExamBeginDateTime), beginDateTime).
                Set(nameof(ExamPaperUser.ExamEndDateTime), endDateTime).
                Where(nameof(ExamPaperUser.ExamPaperId), paperId));
        }
        public async Task UpdateExamTimesAsync(int paperId, int examTimes)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamPaperUser.ExamTimes), examTimes).
                Where(nameof(ExamPaperUser.ExamPaperId), paperId));
        }
        public async Task UpdateLockedAsync(int paperId, bool locked)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamPaperUser.Locked), locked).
                Where(nameof(ExamPaperUser.ExamPaperId), paperId));
        }
        public async Task UpdateLockedAppAsync(int paperId, bool locked)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamPaperUser.LockedApp), locked).
                Where(nameof(ExamPaperUser.ExamPaperId), paperId));
        }
        public async Task UpdateMoniAsync(int paperId, bool moni)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamPaperUser.Moni), moni).
                Where(nameof(ExamPaperUser.ExamPaperId), paperId));
        }
        public async Task UpdateKeyWordsAsync(int paperId, string keyWords)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamPaperUser.KeyWords), keyWords).
                Where(nameof(ExamPaperUser.ExamPaperId), paperId));
        }


        public async Task<int> CountAsync(int paperId)
        {
            return await _repository.CountAsync(Q.Where(nameof(ExamPaperUser.ExamPaperId), paperId));
        }
    }
}
