using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface ICreateManager
    {
        Task ExecuteSubmitAnswerAsync(ExamPaperAnswer examPaperAnswer);
    }
}
