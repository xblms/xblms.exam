using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamTmTreeRepository : IRepository
    {
        Task<int> InsertAsync(ExamTmTree item);

        Task<bool> UpdateAsync(ExamTmTree item);

        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(List<int> ids);
    }
}
