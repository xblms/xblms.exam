using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;


namespace XBLMS.Repositories
{
    public interface IExamPaperRandomRepository : IRepository
    {
        Task<int> GetOneIdByPaperAsync(int PaperId);
        Task<List<ExamPaperRandom>> GetListByPaperAsync(int paperId);
        Task<List<int>> GetIdsByPaperAsync(int paperId);
        Task<int> InsertAsync(ExamPaperRandom item);
        Task<bool> UpdateAsync(ExamPaperRandom item);
        Task<int> DeleteByPaperAsync(int paperId);
        Task DeleteAsync(int id);

    }
}
