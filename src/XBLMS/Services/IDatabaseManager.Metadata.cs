using System.Threading.Tasks;

namespace XBLMS.Services
{
    public partial interface IDatabaseManager
    {
        Task<(bool success, string errorMessage)> InstallAsync(string companyName, string userName, string password, string email,
            string mobile);

        Task SyncDatabaseAsync();
        Task ClearDatabaseAsync();
    }
}
