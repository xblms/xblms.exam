using System.Threading.Tasks;
using XBLMS.Enums;

namespace XBLMS.Services
{
    public partial interface IAuthManager
    {
        Task AddAdminLogAsync(string action, string summary);

        Task AddAdminLogAsync(string action);

        Task AddUserLogAsync(string action, string summary);

        Task AddUserLogAsync(string action);
        Task AddStatLogAsync(StatType statType, string statTypeStr, int objectId = 0, string objectName = "", object entity = null);
        Task AddStatCount(StatType statType);
    }
}
