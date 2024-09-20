using System.Collections.Generic;
using System.Threading.Tasks;

namespace XBLMS.Repositories
{
    public partial interface IAdministratorRepository
    {
        Task<List<string>> GetRoleNames(int adminId);
    }
}
