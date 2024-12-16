using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Services
{
    public partial interface ISignalRHubManagerMessage
    {
        Task SendMsg(PkRoomResult room);
    }
}
