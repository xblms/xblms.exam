using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStudyCourseFilesGroupRepository : IRepository
    {
        Task<int> InsertAsync(StudyCourseFilesGroup group);

        Task<bool> UpdateAsync(StudyCourseFilesGroup group);
        Task<bool> DeleteAsync(int id);
        Task<StudyCourseFilesGroup> GetAsync(int id);
        Task<bool> IsExistsAsync(int parentId, string groupName);
        Task<List<StudyCourseFilesGroup>> GetListAsync(AdminAuth auth, int parentId);
        Task<List<int>> GetParentIdListAsync(int id);
        Task<List<int>> GetChildIdListAsync(int id);
    }
}
