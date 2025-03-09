using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperAnswerRepository : IExamPaperAnswerRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly Repository<ExamPaperAnswer> _repository;

        public ExamPaperAnswerRepository(ISettingsManager settingsManager,IExamPaperRepository examPaperRepository)
        {
            _settingsManager = settingsManager;
            _examPaperRepository = examPaperRepository;
            _repository = new Repository<ExamPaperAnswer>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<int> InsertAsync(ExamPaperAnswer item)
        {
            var tableName = await GetTableNameAsync(item.ExamPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamPaperAnswer item)
        {
            var tableName = await GetTableNameAsync(item.ExamPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.UpdateAsync(item);
        }
        public async Task ClearByPaperAsync(int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            await repository.DeleteAsync(Q.Where(nameof(ExamPaperAnswer.ExamPaperId), examPaperId));
        }
        public async Task DeleteByUserId(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperAnswer.UserId), userId));
        }

        public async Task ClearByPaperAndUserAsync(int examPaperId, int userId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            await repository.DeleteAsync(Q.Where(nameof(ExamPaperAnswer.ExamPaperId), examPaperId).Where(nameof(ExamPaperAnswer.UserId), userId));
        }

        public async Task<ExamPaperAnswer> GetAsync(int id,int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.GetAsync(Q.Where(nameof(ExamPaperAnswer.Id), id));
        }
        public async Task<ExamPaperAnswer> GetAsync(int tmId, int startId, int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.GetAsync(Q.
                Where(nameof(ExamPaperAnswer.ExamPaperId), examPaperId).
                Where(nameof(ExamPaperAnswer.ExamStartId), startId).
                Where(nameof(ExamPaperAnswer.RandomTmId), tmId));
        }


        public async Task<decimal> ScoreSumAsync(int startId,int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            var scoreList = await repository.GetAllAsync<decimal>(Q.
                Select(nameof(ExamPaperAnswer.Score)).
                Where(nameof(ExamPaperAnswer.ExamStartId), startId));

            if (scoreList != null && scoreList.Count > 0)
            {
                return scoreList.Sum();
            }

            return 0;
        }
        public async Task<decimal> ObjectiveScoreSumAsync(int startId,int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            var scoreList = await repository.GetAllAsync<decimal>(Q.
              Select(nameof(ExamPaperAnswer.Score)).
              Where(nameof(ExamPaperAnswer.ExamTmType), ExamTmType.Objective.GetValue()).
              Where(nameof(ExamPaperAnswer.ExamStartId), startId));

            if (scoreList != null && scoreList.Count > 0)
            {
                return scoreList.Sum();
            }

            return 0;
        }
        public async Task<decimal> SubjectiveScoreSumAsync(int startId,int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            var scoreList = await repository.GetAllAsync<decimal>(Q.
             Select(nameof(ExamPaperAnswer.Score)).
             Where(nameof(ExamPaperAnswer.ExamTmType), ExamTmType.Subjective.GetValue()).
             Where(nameof(ExamPaperAnswer.ExamStartId), startId));

            if (scoreList != null && scoreList.Count > 0)
            {
                return scoreList.Sum();
            }

            return 0;
        }
        public async Task<(int rightCount, int wrongCount)> CountAsync(int tmId, int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            var query = Q.NewQuery();
            if (examPaperId > 0)
            {
                query.Where(nameof(ExamPaperAnswer.ExamPaperId), examPaperId);
            }

            var rightCount = await repository.CountAsync(query.
                Where(nameof(ExamPaperAnswer.RandomTmId), tmId).
                Where(nameof(ExamPaperAnswer.Score), ">", 0));

            query = Q.NewQuery();
            if (examPaperId > 0)
            {
                query.Where(nameof(ExamPaperAnswer.ExamPaperId), examPaperId);
            }
            var wrongCount = await repository.CountAsync(query.
                Where(nameof(ExamPaperAnswer.RandomTmId), tmId).
                WhereNotNullOrEmpty(nameof(ExamPaperAnswer.Answer)).
                Where(nameof(ExamPaperAnswer.Score), 0));

            return (rightCount, wrongCount);
        }
    }
}
