using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;


namespace XBLMS.Repositories
{
    public partial interface IExamPaperRandomTmSmallRepository : IRepository
    {
        Task<ExamPaperRandomTmSmall> GetAsync(int Id);
        Task<int> InsertAsync(ExamPaperRandomTmSmall item);
        Task<int> DeleteByPaperAsync(int examPaperId);
        Task<int> DeleteByRandomIdAsync(int examPaperRandomId, int examPaperId);
        Task<List<ExamPaperRandomTmSmall>> GetListAsync(int randomTmId);
        Task<List<ExamPaperRandomTmSmall>> GetListAsync(int examPaperRandomId, int txId, int examPaperId);
        Task<List<int>> GetIdsAsync(int tmId, int examPaperId);
    }
}
