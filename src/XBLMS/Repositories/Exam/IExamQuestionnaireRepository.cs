using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamQuestionnaireRepository : IRepository
    {
        Task<ExamQuestionnaire> GetAsync(int id);
        Task<ExamQuestionnaire> GetAsync(string guId);
        Task<int> InsertAsync(ExamQuestionnaire item);
        Task<bool> UpdateAsync(ExamQuestionnaire item);
        Task<(int total, List<ExamQuestionnaire> list)> GetListAsync(string keyword, int pageIndex, int pageSize);
        Task<bool> DeleteAsync(int Id);
        Task<List<int>> GetIdsAsync(List<int> ids, string keyword);
        Task<int> MaxIdAsync();
        Task IncrementAsync(int id);
    }
}
