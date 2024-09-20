using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPracticeCollectRepository : IRepository
    {
        Task<ExamPracticeCollect> GetAsync(int userId);

        Task<int> InsertAsync(ExamPracticeCollect item);

        Task UpdateAsync(ExamPracticeCollect item);

    }
}
