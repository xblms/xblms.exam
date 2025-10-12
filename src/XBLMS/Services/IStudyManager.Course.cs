using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IStudyManager
    {
        Task User_GetCourseInfoByCourseList(int planId, StudyCourse course, StudyCourseUser courseUser);
    }

}
