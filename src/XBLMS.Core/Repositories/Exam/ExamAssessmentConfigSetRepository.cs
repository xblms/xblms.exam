using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamAssessmentConfigSetRepository : IExamAssessmentConfigSetRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<ExamAssessmentConfigSet> _repository;

        public ExamAssessmentConfigSetRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<ExamAssessmentConfigSet>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(ExamAssessmentConfigSet item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(ExamAssessmentConfigSet item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<bool> DeleteByConfigIdAsync(int configId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(ExamAssessmentConfigSet.ConfigId), configId)) > 0;
        }


        public async Task<ExamAssessmentConfigSet> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }


        public async Task<List<ExamAssessmentConfigSet>> GetListAsync(int configId)
        {
            var query = Q.Where(nameof(ExamAssessmentConfigSet.ConfigId), configId);
            query.OrderBy(nameof(ExamAssessmentConfigSet.Id));
            var list = await _repository.GetAllAsync(query);
            return list;
        }

        public async Task<int> GetCountAsync(int configId)
        {
            var query = Q.Where(nameof(ExamAssessmentConfigSet.ConfigId), configId);
            return await _repository.CountAsync(query);
        }
    }
}
