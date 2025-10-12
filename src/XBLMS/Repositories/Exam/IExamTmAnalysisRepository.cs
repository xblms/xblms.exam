using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamTmAnalysisRepository : IRepository
    {
        Task<bool> ExistsAsync(int id);
        Task<ExamTmAnalysis> GetAsync(TmAnalysisType type, int paperId, int companyId);
        Task<List<ExamTmAnalysis>> GetListAsync();
        Task<int> InsertAsync(ExamTmAnalysis item);
        Task UpdateAsync(ExamTmAnalysis item);
        Task DeleteAsync(int id);
    }
}
