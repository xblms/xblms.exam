using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class StudyCourseEvaluationItemRepository : IStudyCourseEvaluationItemRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyCourseEvaluationItem> _repository;

        public StudyCourseEvaluationItemRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyCourseEvaluationItem>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(StudyCourseEvaluationItem item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(StudyCourseEvaluationItem item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<bool> DeleteByEvaluationIdAsync(int evaluationId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(StudyCourseEvaluationItem.EvaluationId), evaluationId)) > 0;
        }


        public async Task<StudyCourseEvaluationItem> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }


        public async Task<List<StudyCourseEvaluationItem>> GetListAsync(int evaluationId)
        {
            var query = Q.Where(nameof(StudyCourseEvaluationItem.EvaluationId), evaluationId);
            query.OrderByDesc(nameof(StudyCourseEvaluationItem.Taxis));
            var list = await _repository.GetAllAsync(query);
            return list;
        }
    }
}
