using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IUserRepository : IRepository
    {
        Task<(bool success, string errorMessage)> ValidateAsync(string userName, string email, string mobile, string password);
        Task<(User user, string errorMessage)> InsertAsync(User user, string password, bool isChecked, string ipAddress);
        Task UpdateByPkRoomAsync(User user);
        Task<(bool success, string errorMessage)> UpdateAsync(User user);

        Task UpdateLastActivityDateAndCountOfLoginAsync(User user);

        Task UpdateLastActivityDateAsync(User user);

        Task<(bool success, string errorMessage)> ChangePasswordAsync(int userId, string password);


        Task LockAsync(IList<int> userIds);

        Task UnLockAsync(IList<int> userIds);

        Task<bool> IsUserNameExistsAsync(string userName);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsEmailExistsAsync(string email);

        Task<bool> IsMobileExistsAsync(string mobile);

        Task<(User user, string userName, string errorMessage)> ValidateAsync(string account, string password,
            bool isPasswordMd5);

        Task<(bool success, string errorMessage)> ValidateStateAsync(User user);

        Task<int> GetCountByUserGroupAsync();
        Task<int> GetCountByUserGroupAsync(List<int> userIds);
        Task<int> GetCountByUserGroupAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds);
        Task<int> GetCountAsync(int companyId, int departmentId, int dutyId);
        Task<int> GetCountAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds);
        Task<int> GetCountAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int dayOfLastActivity, string keyword);

        Task<List<User>> GetUsersAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int dayOfLastActivity, string keyword, string order,
            int offset, int limit);

        Task<User> DeleteAsync(int userId);

        Task<(int total, List<User> list)> GetListAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, string keyWords, int pageIndex, int pageSize);
        Task<int> GetCountAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int range, int dayOfLastActivity, string keyword);
        Task<List<User>> GetUsersAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int range, int dayOfLastActivity, string keyword, string order,
        int offset, int limit);
        Task<List<int>> GetUserIdsAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int range, int dayOfLastActivity, string keyword, string order);
        Task<List<int>> GetUserIdsAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int dayOfLastActivity, string keyword, string order);
        Task<List<int>> GetUserIdsAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds);
        Task<List<int>> GetUserIdsWithOutLockedAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds);
        Task<List<int>> GetUserIdsWithOutLockedAsync();
    }
}
