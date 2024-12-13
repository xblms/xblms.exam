using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPracticeRepository : IRepository
    {
        Task<List<ExamPractice>> GetListAsync();
        Task<ExamPractice> GetAsync(int id);
        Task<(int total, List<ExamPractice> list)> GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize);
        Task<bool> UpdateAsync(ExamPractice item);
        Task<int> InsertAsync(ExamPractice item);
        Task IncrementAnswerCountAsync(int id);
        Task IncrementRightCountAsync(int id);
        Task DeleteAsync(int userId);
        Task<(int answerTotal, int rightTotal, int allAnswerTotal, int allRightTotal, int collectAnswerTotal, int collectRightTotal, int wrongAnswerTotal, int wrongRightTotal)> SumAsync(int userId);
    }
}
