using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamTmTreeRepository
    {
        Task<string> GetPathNamesAsync(int id);
        Task<List<ExamTmTree>> GetListAsync();
        Task<ExamTmTree> GetAsync(int id);
        Task<List<int>> GetIdsAsync(int id);
    }
}
