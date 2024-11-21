using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamTmGroupRepository : IRepository
    {
        Task<int> InsertAsync(ExamTmGroup group);

        Task UpdateAsync(ExamTmGroup group);

        Task DeleteAsync(int groupId);

        Task ClearCache();
        Task ResetAsync();
    }
}
