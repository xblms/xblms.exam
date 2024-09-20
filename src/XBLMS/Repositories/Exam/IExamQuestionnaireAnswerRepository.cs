using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public  interface IExamQuestionnaireAnswerRepository : IRepository
    {
        Task<int> InsertAsync(ExamQuestionnaireAnswer item);
        Task<bool> UpdateAsync(ExamQuestionnaireAnswer item);
        Task ClearByUserAsync(int userId);
        Task ClearByPaperAsync(int paperId);
        Task ClearByPaperAndUserAsync(int paperId, int userId);
        Task DeleteAsync(int id);
    }
}
