using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamQuestionnaireUserRepository : IExamQuestionnaireUserRepository
    {
        private readonly Repository<ExamQuestionnaireUser> _repository;
        private readonly string _cacheKey;

        public ExamQuestionnaireUserRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamQuestionnaireUser>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<int> InsertAsync(ExamQuestionnaireUser item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamQuestionnaireUser item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task ClearByUserAsync(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamQuestionnaireUser.UserId), userId));
        }
        public async Task ClearByPaperAsync(int paperId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamQuestionnaireUser.ExamPaperId), paperId));
        }

        public async Task ClearByPaperAndUserAsync(int paperId, int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamQuestionnaireUser.ExamPaperId), paperId).Where(nameof(ExamQuestionnaireUser.UserId), userId));
        }
        public async Task<bool> ExistsAsync(int paperId, int userId)
        {
            return await _repository.ExistsAsync(Q.Where(nameof(ExamQuestionnaireUser.ExamPaperId), paperId).Where(nameof(ExamQuestionnaireUser.UserId), userId));
        }

        public async Task<List<int>> GetPaperIdsByUser(int userId)
        {
            var query = Q.
                Select(nameof(ExamQuestionnaireUser.ExamPaperId)).
                Where(nameof(ExamQuestionnaireUser.UserId), userId);



            return await _repository.GetAllAsync<int>(query);
        }

        public async Task<ExamQuestionnaireUser> GetAsync(int paperId, int userId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(ExamQuestionnaireUser.ExamPaperId), paperId).
                Where(nameof(ExamQuestionnaireUser.UserId), userId));
        }
        public async Task<ExamQuestionnaireUser> GetOnlyOneAsync(int userId)
        {
            var query = Q.
                Where(nameof(ExamQuestionnaireUser.UserId), userId);

            return await _repository.GetAsync(query.
                OrderByDesc(nameof(ExamQuestionnaireUser.Id)).
                Limit(1));
        }


        public async Task<List<int>> GetPaperIdsAsync(int userId)
        {
            var query = Q.
                Select(nameof(ExamQuestionnaireUser.ExamPaperId)).
                Where(nameof(ExamQuestionnaireUser.UserId), userId).
                Where(nameof(ExamQuestionnaireUser.SubmitType),SubmitType.Save.GetValue());

            return await _repository.GetAllAsync<int>(query);
        }
    }
}
