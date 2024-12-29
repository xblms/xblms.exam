using Datory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamAssessmentUserRepository : IExamAssessmentUserRepository
    {
        private readonly Repository<ExamAssessmentUser> _repository;
        private readonly string _cacheKey;

        public ExamAssessmentUserRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamAssessmentUser>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<int> InsertAsync(ExamAssessmentUser item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamAssessmentUser item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task ClearByPaperAsync(int assId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamAssessmentUser.ExamAssId), assId));
        }
        public async Task<bool> ExistsAsync(int assId, int userId)
        {
            return await _repository.ExistsAsync(Q.Where(nameof(ExamAssessmentUser.ExamAssId), assId).Where(nameof(ExamAssessmentUser.UserId), userId));
        }

        public async Task<(int total, List<ExamAssessmentUser> list)> GetListAsync(int userId, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.
                WhereNullOrFalse(nameof(ExamAssessmentUser.Locked)).
                Where(nameof(ExamAssessmentUser.UserId),userId);

            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                keyWords = $"%{keyWords}%";
                query.WhereLike(nameof(ExamAssessmentUser.KeyWords), keyWords);
            }
            query.OrderByDesc(nameof(ExamAssessmentUser.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

        public async Task<(int total, List<ExamAssessmentUser> list)> GetListAsync(int assId, string isSubmit, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.
                Where(nameof(ExamAssessmentUser.ExamAssId), assId);

            if (!string.IsNullOrEmpty(isSubmit))
            {
                if (isSubmit == "1")
                {
                    query.Where(nameof(ExamAssessmentUser.SubmitType), SubmitType.Submit.GetValue());
                }
                if (isSubmit == "0")
                {
                    query.WhereNot(nameof(ExamAssessmentUser.SubmitType), SubmitType.Submit.GetValue());
                }
            }

            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                keyWords = $"%{keyWords}%";
                query.WhereLike(nameof(ExamAssessmentUser.KeyWordsAdmin), keyWords);
            }
            query.OrderByDesc(nameof(ExamAssessmentUser.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

        public async Task<ExamAssessmentUser> GetAsync(int assId, int userId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(ExamAssessmentUser.ExamAssId), assId).
                Where(nameof(ExamAssessmentUser.UserId), userId));
        }

        public async Task UpdateLockedAsync(int assId, bool locked)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamAssessmentUser.Locked), locked).
                Where(nameof(ExamAssessmentUser.ExamAssId), assId));
        }
        public async Task UpdateKeyWordsAsync(int assId, string keyWords)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamAssessmentUser.KeyWords), keyWords).
                Where(nameof(ExamAssessmentUser.ExamAssId), assId));
        }

        public async Task UpdateExamDateTimeAsync(int assId, DateTime beginDateTime, DateTime endDateTime)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamAssessmentUser.ExamBeginDateTime), beginDateTime).
                Set(nameof(ExamAssessmentUser.ExamEndDateTime), endDateTime).
                Where(nameof(ExamAssessmentUser.ExamAssId), assId));
        }


        public async Task<(int total,int submitTotal)> GetCountAsync(int assId)
        {
            var query = Q.Where(nameof(ExamAssessmentUser.ExamAssId), assId);

            var total = await _repository.CountAsync(query);
            var submitTotal = await _repository.CountAsync(query.Where(nameof(ExamAssessmentUser.SubmitType), SubmitType.Submit.GetValue()));
            return (total, submitTotal);
        }

        public async Task<(int total,List<ExamAssessmentUser> list)> GetTaskAsync(int userId)
        {
            var query = Q.
                WhereNullOrFalse(nameof(ExamAssessmentUser.Locked)).
                WhereNot(nameof(ExamAssessmentUser.SubmitType), SubmitType.Submit.GetValue()).
                Where(nameof(ExamAssessmentUser.ExamBeginDateTime),"<",DateTime.Now).
                Where(nameof(ExamAssessmentUser.ExamEndDateTime), ">", DateTime.Now).
                Where(nameof(ExamAssessmentUser.UserId), userId);

            var total= await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query);
            return (total, list);
        }
    }
}
