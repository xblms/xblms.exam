using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class StudyCourseWareUserRepository
    {
        public async Task<int> Analysis_GetTotalDurationAsync(int userId, string dateFrom, string dateTo)
        {
            var query = Q.Where(nameof(StudyCourseWareUser.UserId), userId);

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(StudyCourseWareUser.LastModifiedDate), ">=", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(StudyCourseWareUser.LastModifiedDate), "<=", dateTo);
            }

            return await _repository.SumAsync(nameof(StudyCourseWareUser.TotalDuration), query);
        }
        public async Task<int> Analysis_GetTotalDurationAsync(int userId, int planId,int courseId)
        {
            var query = Q.
                Where(nameof(StudyCourseWareUser.PlanId), planId).
                Where(nameof(StudyCourseWareUser.CourseId), courseId).
                Where(nameof(StudyCourseWareUser.UserId), userId);

            return await _repository.SumAsync(nameof(StudyCourseWareUser.TotalDuration), query);
        }
    }
}
