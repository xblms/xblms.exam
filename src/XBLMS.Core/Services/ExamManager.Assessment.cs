using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task ClearExamAssessment(int assId)
        {
            await _databaseManager.ExamAssessmentAnswerRepository.ClearByPaperAsync(assId);
            await _databaseManager.ExamAssessmentTmRepository.DeleteByPaperAsync(assId);
            await _databaseManager.ExamAssessmentUserRepository.ClearByPaperAsync(assId);
        }

        public async Task SerExamAssessmentTm(List<ExamAssessmentTm> tmList, int assId)
        {
            if (tmList != null && tmList.Count > 0)
            {
                foreach (var tm in tmList)
                {
                    tm.ExamAssId = assId;
                    await _databaseManager.ExamAssessmentTmRepository.InsertAsync(tm);
                }
            }
        }
    }
}
