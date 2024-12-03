using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamAssessmentConfigSetRepository : IRepository
    {
        Task<int> InsertAsync(ExamAssessmentConfigSet item);
        Task<bool> UpdateAsync(ExamAssessmentConfigSet item);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByConfigIdAsync(int configId);
        Task<ExamAssessmentConfigSet> GetAsync(int id);
        Task<int> GetCountAsync(int configId);
        Task<List<ExamAssessmentConfigSet>> GetListAsync(int configId);
    }
}
