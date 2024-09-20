using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamTmGroupRepository
    {
        Task<ExamTmGroup> GetAsync(int id);
        Task<List<ExamTmGroup>> GetListAsync();
        Task<List<ExamTmGroup>> GetListWithoutLockedAsync();
    }
}
