using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamTxRepository : IRepository
    {
        Task<bool> IsExistsAsync(string name);

        Task<int> InsertAsync(ExamTx examTx);

        Task UpdateAsync(ExamTx examTx);

        Task DeleteAsync(int id);
        Task ResetAsync();
    }
}
