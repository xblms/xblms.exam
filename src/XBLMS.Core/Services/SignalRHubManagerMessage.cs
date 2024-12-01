using Datory;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class SignalRHubManagerMessage : ISignalRHubManagerMessage
    {
        private readonly IAuthManager _authManager;
        private readonly IHubContext<SignalRHubManager> _hubContext;
        private readonly IExamManager _examManager;
        private readonly IExamTmRepository _examTmRepository;

        public SignalRHubManagerMessage(IAuthManager authManager, IExamManager examManager,IExamTmRepository examTmRepository, IHubContext<SignalRHubManager> hubContext)
        {
            _authManager = authManager;
            _examManager = examManager;
            _examTmRepository = examTmRepository;
            _hubContext = hubContext;
        }
        public async Task SendMsg(PkRoomResult room)
        {
            room.NoticeType = PkRoomNoticeType.None;
            room.NoticeContent = string.Empty;

            if (room.AUserState == PkRoomUserState.Ready && room.BUserState == PkRoomUserState.Ready)
            {
                room.NoticeType = PkRoomNoticeType.ReadyAnswer;
                room.NoticeContent = "请开始答题";

                room.AUserState = room.BUserState = PkRoomUserState.Answer;
                room.AUserAnswer = room.BUserAnswer = string.Empty;
            }
            else if (room.AUserState == PkRoomUserState.AnswerLock && room.BUserState == PkRoomUserState.AnswerLock)
            {
                room.AnswerTotal++;

                var tm = await _examTmRepository.GetAsync(room.TmId);
                if (tm.Answer == room.AUserAnswer)
                {
                    room.AUserAnswerRightTotal++;
                }
                if (tm.Answer == room.BUserAnswer)
                {
                    room.BUserAnswerRightTotal++;
                }

                room.AnswerTmIds.Add(room.TmId);


                room.AAnswers.Add(room.AUserAnswer);
                room.BAnswers.Add(room.BUserAnswer);

                if (room.AUserAnswerRightTotal == 10 || room.BUserAnswerRightTotal == 10)
                {
                    room.NoticeType = PkRoomNoticeType.Finished;
                }
                else
                {
                    room.NoticeType = PkRoomNoticeType.ReadyAnswer;
                    room.NoticeContent = "请开始答题";
                    room.AUserState = room.BUserState = PkRoomUserState.Answer;
                    room.AUserAnswer = room.BUserAnswer = string.Empty;

                    room.TmIndex++;
            
                }
            }

            room.AUserStateStr = room.AUserState.GetDisplayName();
            room.BUserStateStr = room.BUserState.GetDisplayName();

            await _hubContext.Clients.All.SendAsync($"pkroom{room.RoomId}", TranslateUtils.JsonSerialize(room));
        }
    }
}
