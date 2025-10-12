using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStudyCourseWareRepository : IRepository
    {
        Task<int> InsertAsync(StudyCourseWare item);
        Task<bool> UpdateAsync(StudyCourseWare item);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByNotIdsAsync(List<int> notIds, int courseId);
        Task<bool> DeleteByCourseIdAsync(int courseId);
        Task<StudyCourseWare> GetAsync(int id);
        Task<List<StudyCourseWare>> GetListAsync(int courseId);
    }
}
