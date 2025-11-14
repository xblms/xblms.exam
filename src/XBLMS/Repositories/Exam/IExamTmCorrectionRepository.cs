using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamTmCorrectionRepository : IRepository
    {
        Task<(int total, List<ExamTmCorrection> list)> GetListAsync(int userId, int tmId);
        Task<(int total, List<ExamTmCorrection> list)> GetListAsync(int userId, string keyWords,string dateFrom,string dateTo, int pageIndex, int pageSize);
        Task<(int total, List<ExamTmCorrection> list)> GetListAsync(AdminAuth auth, string status, string keyWords, int pageIndex, int pageSize);
        Task<ExamTmCorrection> GetAsync(int id);
        Task<int> InsertAsync(ExamTmCorrection info);
        Task<bool> UpdateAsync(ExamTmCorrection info);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
