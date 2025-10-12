using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperAnswerRepository : IRepository
    {
        Task<int> InsertAsync(ExamPaperAnswer item);
        Task<bool> UpdateAsync(ExamPaperAnswer item);
        Task DeleteByUserId(int userId);
        Task ClearByPaperAsync(int examPaperId);
        Task ClearByPaperAndUserAsync(int examPaperId, int userId);
        Task<ExamPaperAnswer> GetAsync(int id, int examPaperId);
        Task<ExamPaperAnswer> GetAsync(int tmId, int startId, int examPaperId);
        Task<decimal> ScoreSumAsync(int startId, int examPaperId);
        Task<decimal> ObjectiveScoreSumAsync(int startId, int examPaperId);
        Task<decimal> SubjectiveScoreSumAsync(int startId, int examPaperId);
        Task<(int rightCount, int wrongCount)> CountAsync(int tmId, int examPaperId);
    }
}
