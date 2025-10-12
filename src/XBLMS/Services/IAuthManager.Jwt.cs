using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IAuthManager
    {
        string AuthenticateAdministrator(Administrator administrator, bool isPersistent);

        string AuthenticateUser(User user, bool isPersistent);
    }
}
