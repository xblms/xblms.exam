using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public  interface IExamPaperAnswerRepository : IRepository
    {
        Task<int> InsertAsync(ExamPaperAnswer item);
        Task<bool> UpdateAsync(ExamPaperAnswer item);
        Task DeleteByUserId(int userId);
        Task ClearByPaperAsync(int paperId);
        Task ClearByPaperAndUserAsync(int paperId, int userId);
        Task<ExamPaperAnswer> GetAsync(int id);
        Task<ExamPaperAnswer> GetAsync(int tmId, int startId, int paperId);
        Task<decimal> ScoreSumAsync(int startId);
        Task<decimal> ObjectiveScoreSumAsync(int startId);
        Task<decimal> SubjectiveScoreSumAsync(int startId);
        Task<(int rightCount, int wrongCount)> CountAsync(int tmId, int paperId);
    }
}
