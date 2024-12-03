using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IExamManager
    {
        void GetExamAssessmentInfo(ExamAssessment ass, ExamAssessmentUser assUser, User user);
        Task ClearExamAssessment(int assId);
        Task ArrangerExamAssessment(ExamAssessment ass);
        Task SerExamAssessmentTm(List<ExamAssessmentTm> tmList,int assId);
    }

}
