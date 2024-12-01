using Datory;
using Datory.Annotations;
using System.Collections.Generic;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_PkRoom")]
    public class ExamPkRoom : Entity
    {
        [DataColumn]
        public int PkId { get; set; }
        [DataColumn]
        public int UserId_A { get; set; }
        [DataColumn]
        public int UserId_B { get; set; }
        [DataColumn]
        public PkRoomUserState State_A { get; set; }
        [DataColumn]
        public PkRoomUserState State_B { get; set; }
        [DataColumn]
        public int AnswerTotal_A { get; set; }
        [DataColumn]
        public int AnswerTotal_B { get; set; }
        [DataColumn]
        public int RightTotal_A { get; set; }
        [DataColumn]
        public int RightTotal_B { get; set; }
        [DataColumn]
        public int DurationTotal_A { get; set; }
        [DataColumn]
        public int DurationTotal_B { get; set; }
        [DataColumn(Text = true)]
        public List<int> TmIds { get; set; }
        [DataColumn(Text = true)]
        public List<int> AnswerTmIds { get; set; }
        public List<string> AAnswers { get; set; }
        public List<string> BAnswers { get; set; }
        [DataColumn]
        public int WinUserId { get; set; }
        [DataColumn]
        public bool Finished { get; set; }
    }
}
