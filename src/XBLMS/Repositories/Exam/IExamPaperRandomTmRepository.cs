using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;


namespace XBLMS.Repositories
{
    public interface IExamPaperRandomTmRepository : IRepository
    {
        Task<ExamPaperRandomTm> GetAsync(int Id);
        Task<int> InsertAsync(ExamPaperRandomTm item);
        Task<int> DeleteByPaperAsync(int examPaperId);
        Task<List<ExamPaperRandomTm>> GetListAsync(int examPaperRandomId, int txId);
        Task<List<int>> GetIdsAsync(int tmId);
    }
}
