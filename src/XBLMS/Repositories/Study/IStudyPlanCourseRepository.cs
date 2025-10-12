using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStudyPlanCourseRepository : IRepository
    {
        Task<int> InsertAsync(StudyPlanCourse item);
        Task<bool> UpdateAsync(StudyPlanCourse item);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByPlanAsync(int planId);
        Task<bool> DeleteByNotIdsAsync(List<int> notIds, int planId);
        Task<StudyPlanCourse> GetAsync(int id);
        Task<StudyPlanCourse> GetAsync(int planId, int courseId);
        Task<List<StudyPlanCourse>> GetListAsync(bool isSelect, int planId);
        Task<(int total, List<StudyPlanCourse> list)> GetListByTeacherAsync(int teacherId, string keyWords, int pageIndex, int pageSize);
        Task<List<StudyPlanCourse>> GetListAsync(int planId);
        Task<int> CountAsync(int planId, bool isSelect);
        Task<decimal> GetTotalCreditAsync(int planId, bool isSelect);
        Task<int> GetPaperUseCount(int paperId);
        Task<int> GetPaperQUseCount(int paperId);
        Task<int> GetEvaluationUseCount(int eId);
        Task<int> GetCourseUseCount(int courseId);
        Task<(int total, List<StudyPlanCourse> list)> GetOffTrinListByWeekAsync(AdminAuth auth);
    }
}
