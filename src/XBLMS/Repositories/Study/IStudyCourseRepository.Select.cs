using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IStudyCourseRepository
    {
        Task<(int total, List<StudyCourse> list)> Select_GetListAsync(AdminAuth auth, string keyWords, string type, int pageIndex, int pageSize);
    }
}
