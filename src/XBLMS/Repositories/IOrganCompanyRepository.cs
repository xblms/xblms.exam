using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IOrganCompanyRepository : IRepository
    {
        Task<int> InsertAsync(OrganCompany company);
        Task<bool> UpdateAsync(OrganCompany company);
        Task<bool> DeleteByIdsAsync(List<int> ids);
    }
}
