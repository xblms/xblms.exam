using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPracticeAnswerRepository : IRepository
    {
        Task<int> InsertAsync(ExamPracticeAnswer item);
        Task DeleteByTmIdAsync(int tmId);
        Task DeleteByUserId(int userId);
        Task<(int rightCount, int wrongCount)> CountAsync(int tmId);
    }
}
