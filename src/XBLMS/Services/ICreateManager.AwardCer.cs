using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface ICreateManager
    {
        Task AwardCer(ExamPaper paper, int startId, int userId);
    }

}
