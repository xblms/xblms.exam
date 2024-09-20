using System.Threading.Tasks;

namespace XBLMS.Services
{
    public partial interface IAuthManager
    {

        Task AddAdminLogAsync(string action, string summary);

        Task AddAdminLogAsync(string action);

        Task AddUserLogAsync(string action, string summary);

        Task AddUserLogAsync(string action);
    }
}
