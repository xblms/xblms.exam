using System;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IErrorLogRepository
    {
        Task<int> AddErrorLogAsync(ErrorLog log);
        Task<int> AddErrorLogAsync(Exception ex, string summary = "");
    }
}
