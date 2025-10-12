using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamAssessmentConfigRepository : IRepository
    {
        Task<int> InsertAsync(ExamAssessmentConfig item);
        Task<bool> UpdateAsync(ExamAssessmentConfig item);
        Task<bool> DeleteAsync(int id);
        Task<ExamAssessmentConfig> GetAsync(int id);
        Task<int> MaxAsync();
        Task<List<ExamAssessmentConfig>> GetListWithoutLockedAsync(AdminAuth auth);
        Task<(int total, List<ExamAssessmentConfig> list)> GetListAsync(AdminAuth auth, string keyWords, int pageIndex, int pageSize);
    }
}
