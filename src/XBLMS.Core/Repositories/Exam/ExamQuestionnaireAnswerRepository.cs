using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamQuestionnaireAnswerRepository : IExamQuestionnaireAnswerRepository
    {
        private readonly Repository<ExamQuestionnaireAnswer> _repository;
        private readonly string _cacheKey;

        public ExamQuestionnaireAnswerRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamQuestionnaireAnswer>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<int> InsertAsync(ExamQuestionnaireAnswer item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamQuestionnaireAnswer item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task ClearByUserAsync(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamQuestionnaireAnswer.UserId), userId));
        }
        public async Task ClearByPaperAsync(int paperId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamQuestionnaireAnswer.ExamPaperId), paperId));
        }

        public async Task ClearByPaperAndUserAsync(int paperId, int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamQuestionnaireAnswer.ExamPaperId), paperId).Where(nameof(ExamQuestionnaireAnswer.UserId), userId));
        }


        public async Task<List<string>> GetListAnswer(int paperId,int tmId)
        {
            return await _repository.GetAllAsync<string>(Q.
                 Select(nameof(ExamQuestionnaireAnswer.Answer)).
                 Where(nameof(ExamQuestionnaireAnswer.TmId), tmId).
                 Where(nameof(ExamQuestionnaireAnswer.ExamPaperId), paperId).
                 OrderBy(nameof(ExamQuestionnaireAnswer.Id)));
        }
        public async Task<int> GetCountSubmitUser(int paperId,int tmId,string answer)
        {
            answer = $"%{answer}%";

            return await _repository.CountAsync(Q.
                 Select(nameof(ExamQuestionnaireAnswer.Id)).
                 Where(nameof(ExamQuestionnaireAnswer.TmId), tmId).
                 WhereLike(nameof(ExamQuestionnaireAnswer.Answer), answer).
                 Where(nameof(ExamQuestionnaireAnswer.ExamPaperId), paperId));
        }
    }
}
