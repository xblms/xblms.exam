using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamTmAnalysisTmRepository : IRepository
    {
        Task<(int total, List<ExamTmAnalysisTm> list)> GetListAsync(AdminAuth auth, string orderType, string keyWords, int analysisId, int pageIndex, int pageSize);
        Task<int> InsertAsync(ExamTmAnalysisTm item);
        Task DeleteAsync(int analysisId);
        Task DeleteByTmIdAsync(int tmId);

        Task<List<KeyValuePair<int, int>>> GetChatByTxList(int analysisId);
        Task<List<KeyValuePair<int, int>>> GetChatByNdList(int analysisId);
        Task<List<KeyValuePair<string, int>>> GetChatByZsdList(int analysisId);

    }
}
