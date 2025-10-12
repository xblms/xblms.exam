using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IStudyManager
    {
        Task User_GetPlanInfo(StudyPlanUser planUser, bool isDetail = false);
    }

}
