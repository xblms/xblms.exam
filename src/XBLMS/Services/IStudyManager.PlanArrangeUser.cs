using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IStudyManager
    {
        Task PlanArrangeUser(StudyPlan plan, AdminAuth auth);
    }

}
