using System.Collections.Generic;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Dto
{
    public class PkRoomResult
    {
        public PkRoomNoticeType NoticeType { get; set; } = PkRoomNoticeType.None;
        public string NoticeContent { get; set; } = string.Empty;

        public int RoomId { get; set; } = 0;
        public string RoomName { get; set; } = string.Empty;

        public int TmIndex { get; set; } = 0;
        public int TmId { get; set; } = 0;
        public int AnswerTotal { get; set; } = 0;
        public int OnlineUserTotal { get; set; } = 0;
        public List<int> AnswerTmIds { get; set; } = new List<int>();
        public List<string> AAnswers { get; set; } = new List<string>();
        public List<string> BAnswers { get; set; } = new List<string>();


        public int AUserId { get; set; } = 0;
        public string AUserDisplayName { get; set; } = string.Empty;
        public string AUserAvatarUrl { get; set; } = string.Empty;
        public PkRoomUserState AUserState { get; set; } = PkRoomUserState.OffLine;
        public string AUserStateStr { get; set; } = string.Empty;
        public int AUserAnswerRightTotal { get; set; } = 0;
        public string AUserAnswer { get; set; } = string.Empty;
        public int AUserDuration { get; set; } = 0;

        public int BUserId { get; set; } = 0;
        public string BUserDisplayName { get; set; } = string.Empty;
        public string BUserAvatarUrl { get; set; } = string.Empty;
        public PkRoomUserState BUserState { get; set; } = PkRoomUserState.OffLine;
        public string BUserStateStr { get; set; } = string.Empty;
        public int BUserAnswerRightTotal { get; set; } = 0;
        public string BUserAnswer { get; set; } = string.Empty;
        public int BUserDuration { get; set; } = 0;
    }
}
