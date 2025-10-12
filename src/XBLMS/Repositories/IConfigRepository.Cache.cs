using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IConfigRepository
    {
        Task<Config> GetAsync();
        Task<(int value, int maxValue)> GetPointValueByPointType(PointType type);
    }
}
