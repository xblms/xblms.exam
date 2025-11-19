using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IUserRepository : IRepository
    {
        Task<(bool success, string errorMessage)> ValidateAsync(string userName, string email, string mobile, string password);
        Task<(User user, string errorMessage)> InsertAsync(User user, string password, bool isChecked, string ipAddress);
        Task UpdateByPkRoomAsync(User user);
        Task<(bool success, string errorMessage)> UpdateAsync(User user);
        Task UpdateUserGroupIdsAsync(User user);
        Task UpdateUserGroupIdsAsync(int groupId);
        Task UpdateLastActivityDateAndCountOfLoginAsync(User user);

        Task UpdateLastActivityDateAsync(User user);

        Task<(bool success, string errorMessage)> ChangePasswordAsync(int userId, string password);

        Task UpdatePointsAsync(int points, int userId);
        Task UpdatePointsSurplusAsync(int points, int userId);
        Task UpdatePointShopInfoAsync(string linkMan, string linkTel, string linkAddress, int userId);
        Task LockAsync(IList<int> userIds);

        Task UnLockAsync(IList<int> userIds);

        Task<bool> IsUserNameExistsAsync(string userName);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsEmailExistsAsync(string email);

        Task<bool> IsMobileExistsAsync(string mobile);

        Task<(User user, string userName, string errorMessage)> ValidateAsync(string account, string password,
            bool isPasswordMd5);
        Task<(bool success, string errorMessage)> ValidateStateAsync(User user);
        Task<User> DeleteAsync(int userId);
        Task<(int total, List<User> list)> GetListAsync(AdminAuth auth, int organId, string organType, string keyWords, int pageIndex, int pageSize);
        Task<(int total, List<User> list)> GetListAsync(AdminAuth auth, int organId, string organType, UserGroup group, int dayOfLastActivity, string keyword, string order, int pageIndex, int pageSize);
        Task<(int total, int count)> GetCountByCompanyAsync(AdminAuth auth, int companyId);
        Task<(int total, int count)> GetCountByDepartmentAsync(AdminAuth auth, int departmentId);
    }
}
