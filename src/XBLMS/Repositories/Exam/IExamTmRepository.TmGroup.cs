using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamTmRepository
    {
        Task<decimal> Group_GetTotalScoreAsync(ExamTmGroup group);
        Task<int> Group_GetTmTotalAsync(ExamTmGroup group);
        Task<List<ExamTm>> Group_GetTmListAsync(ExamTmGroup group);
        Task<List<int>> Group_GetTmIdsAsync(ExamTmGroup group, List<int> txIds = null);
        Task<List<ExamTm>> Group_GetTmListAsync(ExamTmGroup group, List<int> txIds = null);
        Task<List<int>> Group_RangeIdsAsync(ExamTmGroup group);
        Task<List<int>> Group_Practice_GetTmidsAsync(ExamTmGroup group, List<int> txIds, List<int> nds, List<string> zsds);
        Task<List<int>> Group_Practice_GetTmidsAsync(ExamTmGroup group, List<int> txIds, List<int> nds, string zsd);
        Task<List<string>> Group_Practice_GetZsdsAsync(ExamTmGroup group, List<int> txIds, List<int> nds);
    }
}
