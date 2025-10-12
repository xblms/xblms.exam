using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPracticeAnswerSmallRepository : IRepository
    {
        Task<int> InsertAsync(ExamPracticeAnswerSmall item);
        Task<bool> UpdateAsync(ExamPracticeAnswerSmall item);
        Task<ExamPracticeAnswerSmall> GetAsync(int tmId, int practiceId);
        Task<ExamPracticeAnswerSmall> GetAsync(int id);
        Task<List<ExamPracticeAnswerSmall>> GetListAsync(int answerId, int practiceId);
    }
}
