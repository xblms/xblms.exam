using System.Collections.Generic;
using System.Threading.Tasks;

namespace XBLMS.Services
{
    public partial interface IExamManager
    {
        void ArrangeExamTask(int paperId);
        void ArrangeAssessmentTask(int assId);
        void ArrangeQuestionnaireTask(int qId);
        Task ArrangeOnlyOne(int paperId, int userId);
        Task<List<int>> GetUserIdsByUserGroups(List<int> userGroupIds);
        Task<List<int>> GetTmIdsByTmGroups(List<int> tmGroupIds, List<int> txIds = null);
    }

}
