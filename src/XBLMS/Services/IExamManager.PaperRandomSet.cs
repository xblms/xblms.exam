using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IExamManager
    {
        Task<bool> PaperRandomSet(ExamPaper paper);
        Task SetExamPaperRantomByRandomNowAndExaming(ExamPaper paper, bool isExaming = false);
    }

}
