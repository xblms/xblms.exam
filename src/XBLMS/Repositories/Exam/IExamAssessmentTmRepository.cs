using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;


namespace XBLMS.Repositories
{
    public interface IExamAssessmentTmRepository : IRepository
    {
        Task<ExamAssessmentTm> GetAsync(int Id);
        Task<int> InsertAsync(ExamAssessmentTm item);
        Task<int> DeleteByPaperAsync(int assId);
        Task<List<ExamAssessmentTm>> GetListAsync(int assId);
    }
}
