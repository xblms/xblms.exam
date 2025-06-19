using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;


namespace XBLMS.Repositories
{
    public interface IExamTmSmallRepository : IRepository
    {
        Task<ExamTmSmall> GetAsync(int id);
        Task<int> InsertAsync(ExamTmSmall item);
        Task<bool> UpdateAsync(ExamTmSmall item);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByParentIdAsync(int parentId);
        Task<List<ExamTmSmall>> GetListAsync(int parentId);
    }
}
