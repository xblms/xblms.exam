using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPracticeAnswerRepository : IRepository
    {
        Task<int> InsertAsync(ExamPracticeAnswer item);
    }
}
