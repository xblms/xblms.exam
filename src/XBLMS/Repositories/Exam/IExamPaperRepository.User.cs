using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperRepository : IRepository
    {
        Task<(int total, List<ExamPaper> list)> GetListByUserAsync(List<int> paperIds, string keyWords, int pageIndex, int pageSize, bool isMoni = false);
    }
}
