using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperTreeRepository
    {
        Task<List<ExamPaperTree>> GetAllAsync();
        Task<List<string>> GetParentPathAsync(int id);
        Task<List<ExamPaperTree>> GetListAsync(AdminAuth auth);
        Task<ExamPaperTree> GetAsync(int id);
        Task<List<int>> GetIdsAsync(int id);
    }
}
