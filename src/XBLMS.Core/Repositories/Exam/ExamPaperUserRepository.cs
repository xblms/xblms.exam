using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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

        public async Task ClearByUserAsync(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperUser.UserId), userId));
        }
        public async Task ClearByPaperAsync(int paperId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperUser.ExamPaperId), paperId));
        }

        public async Task ClearByPaperAndUserAsync(int paperId, int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperUser.ExamPaperId), paperId).Where(nameof(ExamPaperUser.UserId), userId));
        }
        public async Task<bool> ExistsAsync(int paperId, int userId)
        {
            return await _repository.ExistsAsync(Q.Where(nameof(ExamPaperUser.ExamPaperId), paperId).Where(nameof(ExamPaperUser.UserId), userId));
        }

        public async Task<List<int>> GetPaperIdsByUser(int userId, string date)
        {
            var query = Q.
                Select(nameof(ExamPaperUser.ExamPaperId)).
                Where(nameof(ExamPaperUser.UserId), userId);

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

                query.Where(nameof(ExamPaperUser.ExamBeginDateTime), ">=", dateFromStr);
                query.Where(nameof(ExamPaperUser.ExamBeginDateTime), "<=", dateToStr);
            }



            return await _repository.GetAllAsync<int>(query);
        }

        public async Task<ExamPaperUser> GetAsync(int paperId, int userId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(ExamPaperUser.ExamPaperId), paperId).
                Where(nameof(ExamPaperUser.UserId), userId));
        }
        public async Task<ExamPaperUser> GetOnlyOneAsync(int userId)
        {
            var query = Q.
                Where(nameof(ExamPaperUser.UserId), userId);

            return await _repository.GetAsync(query.
                OrderByDesc(nameof(ExamPaperUser.Id)).
                Limit(1));
        }
    }
}
