using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperTreeRepository : IRepository
    {
        Task<int> InsertAsync(ExamPaperTree item);

        Task<bool> UpdateAsync(ExamPaperTree item);

        Task<bool> DeleteAsync(List<int> ids);
    }
}
