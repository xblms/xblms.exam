using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface ICreateManager
    {
        void CreateSubmitAnswerAsync(ExamPaperAnswer answer);
        void CreateSubmitPaperAsync(int startId);
    }
}
