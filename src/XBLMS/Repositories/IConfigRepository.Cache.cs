using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IConfigRepository
    {
        Task<Config> GetAsync();
    }
}
