using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamAssessmentTmRepository : IExamAssessmentTmRepository
    {
        private readonly Repository<ExamAssessmentTm> _repository;

        public ExamAssessmentTmRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamAssessmentTm>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<ExamAssessmentTm> GetAsync(int Id)
        {
            return await _repository.GetAsync(Id);
        }


        public async Task<int> InsertAsync(ExamAssessmentTm item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<int> DeleteByPaperAsync(int assId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(ExamAssessmentTm.ExamAssId), assId));
        }

        public async Task<List<ExamAssessmentTm>> GetListAsync(int assId)
        {
            var infoList = await _repository.GetAllAsync(Q.
                Where(nameof(ExamAssessmentTm.ExamAssId), assId).
                OrderBy(nameof(ExamAssessmentTm.Id)));
            return infoList;
        }

    }
}
