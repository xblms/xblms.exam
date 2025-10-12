using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStudyCourseEvaluationItemRepository : IRepository
    {
        Task<int> InsertAsync(StudyCourseEvaluationItem item);
        Task<bool> UpdateAsync(StudyCourseEvaluationItem item);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByEvaluationIdAsync(int evaluationId);
        Task<StudyCourseEvaluationItem> GetAsync(int id);
        Task<List<StudyCourseEvaluationItem>> GetListAsync(int evaluationId);
    }
}
