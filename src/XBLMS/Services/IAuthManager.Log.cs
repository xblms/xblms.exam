using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IAuthManager
    {
        Task AddAdminLogAsync(string action, string summary);

        Task AddAdminLogAsync(string action);

        Task AddUserLogAsync(string action, string summary);

        Task AddUserLogAsync(string action);
        Task AddStatLogAsync(StatType statType, string statTypeStr, int objectId = 0, string objectName = "", object entity = null);
        Task AddStatCount(StatType statType, Administrator admin = null);
        Task AddUserStatCount(StatType statType, User user = null);
        Task AddPointsLogAsync(PointType pointType, User user, int objectId = 0, string objectName = "", bool isNotice = false);
        Task<PointNotice> PointNotice(PointType type, int userId);
        Task<PointNotice> PointNotice(int userId);
    }
}
