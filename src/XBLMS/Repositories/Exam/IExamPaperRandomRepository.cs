using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;


namespace XBLMS.Repositories
{
    public partial interface IExamPaperRandomRepository : IRepository
    {
        Task<int> GetOneIdByPaperAsync(int examPaperId);
        Task<List<ExamPaperRandom>> GetListByPaperAsync(int examPaperId);
        Task<List<int>> GetIdsByPaperAsync(int examPaperId);
        Task<int> InsertAsync(ExamPaperRandom item);
        Task<bool> UpdateAsync(ExamPaperRandom item);
        Task<int> DeleteByPaperAsync(int examPaperId);
        Task DeleteAsync(int id, int examPaperId);

    }
}
