using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;


namespace XBLMS.Repositories
{
    public partial interface IExamTmAiRepository : IRepository
    {
        Task<int> InsertAsync(ExamTmAi item);
        Task<bool> UpdateAsync(ExamTmAi item);
        Task<bool> DeleteAsync(int id);
        Task DeleteAllAsync(AdminAuth auth);
        Task StockedAsync(int id);
        Task StockedAllAsync(AdminAuth auth);
        Task<(int total, List<ExamTmAi>)> GetStockedAllAsync(AdminAuth auth);
        Task<ExamTmAi> GetAsync(int id);
        Task<(int total, List<ExamTmAi> list)> GetListAsync(AdminAuth auth, int status, string keyWords, int pageIndex, int pageSize);
    }
}
