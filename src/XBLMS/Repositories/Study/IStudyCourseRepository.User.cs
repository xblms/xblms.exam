using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IStudyCourseRepository
    {
        Task<(int total, List<StudyCourse> list)> User_GetPublicListAsync(int companyId, string keyWords, string mark, string orderby, int pageIndex, int pageSize);
        Task<(int total, List<string>)> User_GetPublicMarkListAsync(int companyId);
        Task<(int total, List<StudyCourse> list)> User_GetPublicEventListAsync(int companyId);
    }
}
