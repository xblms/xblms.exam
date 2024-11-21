using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperAnswerRepository : IExamPaperAnswerRepository
    {
        private readonly Repository<ExamPaperAnswer> _repository;
        private readonly string _cacheKey;

        public ExamPaperAnswerRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPaperAnswer>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<int> InsertAsync(ExamPaperAnswer item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamPaperAnswer item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task ClearByUserAsync(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperAnswer.UserId), userId));
        }
        public async Task ClearByPaperAsync(int paperId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperAnswer.ExamPaperId), paperId));
        }

        public async Task ClearByPaperAndUserAsync(int paperId,int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPaperAnswer.ExamPaperId), paperId).Where(nameof(ExamPaperAnswer.UserId), userId));
        }

        public async Task<ExamPaperAnswer> GetAsync(int id)
        {
            return await _repository.GetAsync(Q.Where(nameof(ExamPaperAnswer.Id), id));
        }
        public async Task<ExamPaperAnswer> GetAsync(int tmId,int startId,int paperId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(ExamPaperAnswer.ExamPaperId), paperId).
                Where(nameof(ExamPaperAnswer.ExamStartId), startId).
                Where(nameof(ExamPaperAnswer.RandomTmId), tmId));
        }


        public async Task<decimal> ScoreSumAsync(int startId)
        {
            var scoreList = await _repository.GetAllAsync<decimal>(Q.
                Select(nameof(ExamPaperAnswer.Score)).
                Where(nameof(ExamPaperAnswer.ExamStartId), startId));

            if(scoreList!=null && scoreList.Count > 0)
            {
                return scoreList.Sum();
            }

            return 0;
        }
        public async Task<decimal> ObjectiveScoreSumAsync(int startId)
        {
            var scoreList = await _repository.GetAllAsync<decimal>(Q.
              Select(nameof(ExamPaperAnswer.Score)).
              Where(nameof(ExamPaperAnswer.ExamTmType), ExamTmType.Objective.GetValue()).
              Where(nameof(ExamPaperAnswer.ExamStartId), startId));

            if (scoreList != null && scoreList.Count > 0)
            {
                return scoreList.Sum();
            }

            return 0;
        }
        public async Task<decimal> SubjectiveScoreSumAsync(int startId)
        {
            var scoreList = await _repository.GetAllAsync<decimal>(Q.
             Select(nameof(ExamPaperAnswer.Score)).
             Where(nameof(ExamPaperAnswer.ExamTmType), ExamTmType.Subjective.GetValue()).
             Where(nameof(ExamPaperAnswer.ExamStartId), startId));

            if (scoreList != null && scoreList.Count > 0)
            {
                return scoreList.Sum();
            }

            return 0;
        }
    }
}
