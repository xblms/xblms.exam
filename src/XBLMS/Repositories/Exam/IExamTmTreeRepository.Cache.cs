using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamTmTreeRepository
    {
        Task<List<ExamTmTree>> GetAllAsync();
        Task<List<string>> GetParentPathAsync(int id);
        Task<string> GetPathNamesAsync(int id);
        Task<List<ExamTmTree>> GetListAsync(AdminAuth auth);
        Task<ExamTmTree> GetAsync(int id);
        Task<List<int>> GetIdsAsync(int id);
    }
}
