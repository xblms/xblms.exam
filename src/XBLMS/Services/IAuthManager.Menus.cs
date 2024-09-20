using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;

namespace XBLMS.Services
{
    public partial interface IAuthManager
    {
        Task<List<Menu>> GetMenus();
    }
}
