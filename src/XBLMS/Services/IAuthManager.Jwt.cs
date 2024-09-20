using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IAuthManager
    {
        string AuthenticateAdministrator(Administrator administrator, bool isPersistent);

        Task<string> RefreshAdministratorTokenAsync(string accessToken);

        string AuthenticateUser(User user, bool isPersistent);

        Task<string> RefreshUserTokenAsync(string accessToken);
    }
}
