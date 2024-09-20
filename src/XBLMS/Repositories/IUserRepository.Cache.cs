using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IUserRepository
    {
        Task<User> GetByAccountAsync(string account);

        Task<User> GetByUserIdAsync(int userId);

        Task<User> GetByUserNameAsync(string userName);

        Task<User> GetByMobileAsync(string mobile);

        Task<User> GetByEmailAsync(string email);

        Task<User> GetByGuidAsync(string guid);

        Task<string> GetDisplayAsync(int userId);

        string GetDisplay(User user);
    }
}
