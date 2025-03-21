using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPracticeAnswerRepository : IRepository
    {
        Task<int> InsertAsync(ExamPracticeAnswer item);
        Task DeleteByTmIdAsync(int tmId);
        Task DeleteByUserId(int userId);
        Task DeleteByPracticeIdAsync(int practiceId);
        Task<(int rightCount, int wrongCount)> CountAsync(int tmId);
        Task<List<ExamPracticeAnswer>> GetListAsync(int practiceId);
    }
}
