using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IAdministratorRepository : IRepository
    {
        Task UpdateAuthAsync(Administrator administrator);
        Task UpdateAuthDataAsync(Administrator administrator);
        Task UpdateCurrentOragnAsync(Administrator administrator);
        Task UpdateAuthDataShowAllAsync(Administrator administrator);
        Task UpdateLastActivityDateAndCountOfFailedLoginAsync(Administrator administrator);

        Task UpdateLastActivityDateAsync(Administrator administrator);

        Task UpdateLastActivityDateAndCountOfLoginAsync(Administrator administrator);
        

        Task LockAsync(IList<string> userNames);

        Task UnLockAsync(IList<string> userNames);
        Task<int> GetCountAsync(int companyId, int departmentId);
        Task<int> GetCountAsync(List<int> companyIds, List<int> departmentIds);
        Task<List<Administrator>> GetListAsync();
        Task<List<Administrator>> GetAllAsync();
        Task<int> GetCountAsync(AdminAuth auth, int organId, string organType, string role, int lastActivityDate, string keyword);

        Task<(int total, List<Administrator> list)> GetAdministratorsAsync(AdminAuth auth, int organId, string organType, string role, string order,
            int lastActivityDate, string keyword, int offset, int limit);

        Task<List<int>> GetAdministratorIdsAsync(string keyword);

        Task<bool> IsUserNameExistsAsync(string adminName);

        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsMobileExistsAsync(string mobile);

        Task<(bool IsValid, string ErrorMessage)> InsertValidateAsync(string userName, string password, string email,
            string mobile);

        Task<(bool IsValid, string ErrorMessage)> InsertAsync(Administrator administrator, string password);

        Task<(bool IsValid, string ErrorMessage)> UpdateAsync(Administrator administrator);

        Task<(bool IsValid, string ErrorMessage)> ChangePasswordAsync(Administrator adminEntity, string password);

        Task<(Administrator administrator, string userName, string errorMessage)> ValidateAsync(string account, string password, bool isPasswordMd5);

        Task<(bool Success, string ErrorMessage)> ValidateLockAsync(Administrator administrator);

        Task<Administrator> DeleteAsync(int id);
        Task<(int total, int count)> GetCountByCompanyAsync(AdminAuth auth, int companyId);
        Task<(int total, int count)> GetCountByDepartmentAsync(AdminAuth auth, int departmentId);
    }
}
