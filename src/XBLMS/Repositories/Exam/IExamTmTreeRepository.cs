using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamTmTreeRepository : IRepository
    {
        Task<int> InsertAsync(ExamTmTree item);
        Task<bool> UpdateAsync(ExamTmTree item);
        Task<bool> DeleteAsync(int id);
    }
}
