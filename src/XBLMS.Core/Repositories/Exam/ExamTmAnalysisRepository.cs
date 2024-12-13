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
    public class ExamTmAnalysisRepository : IExamTmAnalysisRepository
    {
        private readonly Repository<ExamTmAnalysis> _repository;

        public ExamTmAnalysisRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamTmAnalysis>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<ExamTmAnalysis> GetAsync(TmAnalysisType type,int paperId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(ExamTmAnalysis.TmAnalysisExamPapaerId), paperId).
                Where(nameof(ExamTmAnalysis.TmAnalysisType), type.GetValue()));
        }

        public async Task<List<ExamTmAnalysis>> GetListAsync()
        {
            var list = (await _repository.GetAllAsync(Q
                .OrderBy(nameof(ExamTmAnalysis.Id))
            )).ToList();
            return list;
        }

        public async Task<int> InsertAsync(ExamTmAnalysis item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task UpdateAsync(ExamTmAnalysis item)
        {
            await _repository.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
