using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface ICreateManager
    {
        void CreateSubmitAnswerAsync(ExamPaperAnswer answer);
        void CreateSubmitAnswerSmallAsync(ExamPaperAnswerSmall answer);
        void CreateSubmitPaperAsync(int taskId);
        void CreateExamAwardCerAsync(int taskId);
    }
}
