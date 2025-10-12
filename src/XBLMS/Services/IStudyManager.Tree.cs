using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Services
{
    public partial interface IStudyManager
    {
        Task<List<Cascade<int>>> GetStudyCourseTreeCascadesAsync(AdminAuth auth);
    }

}
