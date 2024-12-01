using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_PkRoomAnswer")]
    public class ExamPkRoomAnswer : Entity
    {
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int RoomId { get; set; }
        [DataColumn]
        public int TmId { get; set; }
        [DataColumn]
        public string Answer { get; set; }
    }
}
