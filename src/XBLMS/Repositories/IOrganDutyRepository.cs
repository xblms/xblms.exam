using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IOrganDutyRepository : IRepository
    {
        Task<int> InsertAsync(OrganDuty duty);

        Task<bool> UpdateAsync(OrganDuty duty);

        Task<bool> DeleteAsync(int id);
    }
}
