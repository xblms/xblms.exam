using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IStudyCourseTreeRepository : IRepository
    {
        Task<int> InsertAsync(StudyCourseTree item);

        Task<bool> UpdateAsync(StudyCourseTree item);

        Task<bool> DeleteAsync(int id);

        Task<List<string>> GetParentPathAsync(int id);
        Task<List<StudyCourseTree>> GetListAsync(AdminAuth auth);
        Task<StudyCourseTree> GetAsync(int id);
        Task<List<int>> GetIdsAsync(int id);
    }
}
