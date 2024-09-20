using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPracticeWrongRepository : IRepository
    {
        Task<ExamPracticeWrong> GetAsync(int userId);

        Task<int> InsertAsync(ExamPracticeWrong item);

        Task UpdateAsync(ExamPracticeWrong item);

    }
}
