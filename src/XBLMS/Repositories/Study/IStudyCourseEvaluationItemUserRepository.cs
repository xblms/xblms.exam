using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStudyCourseEvaluationItemUserRepository : IRepository
    {
        Task DeleteByUserId(int userId);
        Task<int> InsertAsync(StudyCourseEvaluationItemUser item);
        Task<StudyCourseEvaluationItemUser> GetAsync(int id);
        Task<StudyCourseEvaluationItemUser> GetAsync(int planId, int courseId, int userId, int evaluationId, int itemId);
        Task<string> GetTextAsync(int courseId, int userId);
    }
}
