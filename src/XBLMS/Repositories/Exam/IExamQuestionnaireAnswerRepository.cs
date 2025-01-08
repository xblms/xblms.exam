using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public  interface IExamQuestionnaireAnswerRepository : IRepository
    {
        Task<int> InsertAsync(ExamQuestionnaireAnswer item);
        Task ClearByPaperAsync(int paperId);
        Task DeleteByUserId(int userId);
        Task<int> GetCountSubmitUser(int paperId, int tmId, string answer);
        Task<List<string>> GetListAnswer(int paperId, int tmId);
    }
}
