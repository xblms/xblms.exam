using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperRepository : IRepository
    {
        Task<(int total, List<ExamPaper> list)> Select_GetListAsync(AdminAuth auth, string keyWords, int pageIndex, int pageSize);
    }
}
