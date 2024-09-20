using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public interface IBlockManager
    {
        List<IdName> GetAreas();

        BlockArea GetArea(int geoNameId);

        int GetGeoNameId(string ipAddress);

        Task<(bool, BlockRule)> IsBlockedAsync(string ipAddress, string sessionId, int block=1);
    }

}
