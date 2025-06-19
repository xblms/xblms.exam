using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperAnswerSmallRepository : IRepository
    {
        Task<ExamPaperAnswerSmall> GetAsync(int id);
        Task<ExamPaperAnswerSmall> GetAsync(int tmId, int startId, int examPaperId);
        Task<List<ExamPaperAnswerSmall>> GetListAsync(int answerId);
        Task<int> InsertAsync(ExamPaperAnswerSmall item);
        Task<bool> UpdateAsync(ExamPaperAnswerSmall item);
    }
}
