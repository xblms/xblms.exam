using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamTmRepository
    {
        List<ExamTm> CacheGetListAsync(int companyId);
        Task<List<ExamTm>> CacheSetListAsync(int companyId);
        void CacheRemoveListAsync(int companyId);
    }
}
