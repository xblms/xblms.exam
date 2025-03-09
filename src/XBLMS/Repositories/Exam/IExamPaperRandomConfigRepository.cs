using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperRandomConfigRepository : IRepository
    {
        Task<int> InsertAsync(ExamPaperRandomConfig item);
        Task<List<ExamPaperRandomConfig>> GetListAsync(int examPaperId);
        Task<int> DeleteByPaperAsync(int examPaperId);
    }
}
