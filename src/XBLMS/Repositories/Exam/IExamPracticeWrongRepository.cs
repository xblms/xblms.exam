using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPracticeWrongRepository : IRepository
    {
        Task<List<ExamPracticeWrong>> GetListAsync();
        Task<ExamPracticeWrong> GetAsync(int userId);
        Task<int> InsertAsync(ExamPracticeWrong item);
        Task UpdateAsync(ExamPracticeWrong item);
        Task DeleteByUserId(int userId);
    }
}
