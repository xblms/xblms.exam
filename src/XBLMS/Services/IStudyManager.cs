using System.Threading.Tasks;

namespace XBLMS.Services
{
    public partial interface IStudyManager
    {
        Task<int> GetUseCountByPaperId(int paperId);
        Task<int> GetUseCountByPaperQId(int paperId);
        Task<int> GetUseCountByEvaluationId(int evaluationId);
    }

}
