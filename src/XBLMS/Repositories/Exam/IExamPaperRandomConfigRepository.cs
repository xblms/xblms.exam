using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public  interface IExamPaperRandomConfigRepository : IRepository
    {
        Task<int> InsertAsync(ExamPaperRandomConfig item);
        Task<bool> UpdateAsync(ExamPaperRandomConfig item);
        Task<List<ExamPaperRandomConfig>> GetListAsync(int examPaperId);
        Task<bool> DeleteAsync(int Id);
        Task<int> DeleteByPaperAsync(int examPaperId);
    }
}
