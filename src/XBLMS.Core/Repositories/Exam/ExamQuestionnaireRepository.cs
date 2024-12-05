using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamQuestionnaireRepository : IExamQuestionnaireRepository
    {
        private readonly Repository<ExamQuestionnaire> _repository;

        public ExamQuestionnaireRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamQuestionnaire>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }
        public async Task<int> InsertAsync(ExamQuestionnaire item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamQuestionnaire item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task<(int total, List<ExamQuestionnaire> list)> GetListAsync(string keyword, int pageIndex, int pageSize)
        {
            var query = new Query();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(ExamQuestionnaire.Title), like)
                );
            }
            query.OrderByDesc(nameof(ExamQuestionnaire.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<ExamQuestionnaire> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<ExamQuestionnaire> GetAsync(string guId)
        {
            return await _repository.GetAsync(Q.Where(nameof(ExamQuestionnaire.Guid), guId));
        }
        public async Task<bool> DeleteAsync(int Id)
        {
            var result = await _repository.DeleteAsync(Id);
            return result;
        }

        public async Task<int> MaxIdAsync()
        {
            var maxId= await _repository.MaxAsync(nameof(ExamQuestionnaire.Id));
            if (maxId.HasValue)
            {
                return maxId.Value;
            }
            return 0;
        }

        public async Task IncrementAsync(int id)
        {
            await _repository.IncrementAsync(nameof(ExamQuestionnaire.AnswerTotal),Q.Where(nameof(ExamQuestionnaire.Id), id));
        }

        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount()
        {
            var count = await _repository.CountAsync();
            var lockedCount = await _repository.CountAsync(Q.WhereTrue(nameof(ExamQuestionnaire.Locked)));
            var unLockedCount = await _repository.CountAsync(Q.WhereNullOrFalse(nameof(ExamQuestionnaire.Locked)));
            return (count, 0, 0, lockedCount, unLockedCount);
        }
    }
}
