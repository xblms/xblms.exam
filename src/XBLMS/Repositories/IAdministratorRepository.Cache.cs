using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IAdministratorRepository
    {
        Task<int> GetMaxId();
        Task<Administrator> GetByAccountAsync(string account);

        Task<Administrator> GetByUserIdAsync(int userId);

        Task<Administrator> GetByUserNameAsync(string userName);

        Task<Administrator> GetByMobileAsync(string mobile);

        Task<Administrator> GetByEmailAsync(string email);

        string GetDisplay(Administrator admin);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth);
    }
}
