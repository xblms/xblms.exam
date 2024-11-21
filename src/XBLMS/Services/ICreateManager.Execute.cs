using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface ICreateManager
    {
        Task ExecuteSubmitAnswerAsync(ExamPaperAnswer examPaperAnswer);
    }
}
