using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamTxRepository
    {
        Task<ExamTx> GetAsync(string txName);
        Task<ExamTx> GetAsync(int id);
        Task<List<ExamTx>> GetListAsync();
    }
}
