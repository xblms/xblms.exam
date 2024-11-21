using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPracticeAnswerRepository : IRepository
    {
        Task<ExamPracticeAnswer> GetAsync(int userId,int tmId,int practiceId);

        Task<int> InsertAsync(ExamPracticeAnswer item);

        Task UpdateAsync(ExamPracticeAnswer item);

    }
}
