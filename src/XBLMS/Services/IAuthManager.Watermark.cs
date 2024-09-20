using System.Threading.Tasks;

namespace XBLMS.Services
{
    public partial interface IAuthManager
    {
        Task<string> GetWatermark();
    }
}
