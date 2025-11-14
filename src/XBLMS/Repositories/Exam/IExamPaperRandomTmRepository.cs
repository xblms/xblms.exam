using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;


namespace XBLMS.Repositories
{
    public partial interface IExamPaperRandomTmRepository : IRepository
    {
        Task<ExamPaperRandomTm> GetAsync(int Id, int examPaperId);
        Task<int> InsertAsync(ExamPaperRandomTm item);
        Task<int> DeleteByPaperAsync(int examPaperId);
        Task<int> DeleteByRandomIdAsync(int examPaperRandomId, int examPaperId);
        Task<List<ExamPaperRandomTm>> GetListAsync(int examPaperRandomId, int txId, int examPaperId);
        Task<List<int>> GetIdsAsync(int tmId, int examPaperId);
        Task UpdateByCorrectionAsync(int examPaperId, ExamTm tm);
    }
}
