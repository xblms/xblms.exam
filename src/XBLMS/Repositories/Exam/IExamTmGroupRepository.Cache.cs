using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamTmGroupRepository
    {
        Task<bool> ExistsAsync(string name, int companyId);
        Task<ExamTmGroup> GetAsync(int id);
        Task<List<ExamTmGroup>> GetListByOpenUserAsync();
        Task<List<ExamTmGroup>> GetListAsync(AdminAuth auth, string keyWords = null, bool withoutLocked = false);
    }
}
