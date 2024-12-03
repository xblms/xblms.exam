using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public  interface IExamAssessmentAnswerRepository : IRepository
    {
        Task<int> InsertAsync(ExamAssessmentAnswer item);
        Task<bool> UpdateAsync(ExamAssessmentAnswer item);
        Task ClearByUserAsync(int userId);
        Task ClearByPaperAsync(int assId);
        Task ClearByPaperAndUserAsync(int assId, int userId);
        Task DeleteAsync(int id);
        Task<int> GetCountSubmitUser(int assId, int tmId, string answer);
        Task<List<string>> GetListAnswer(int assId, int tmId);
 
    }
}
