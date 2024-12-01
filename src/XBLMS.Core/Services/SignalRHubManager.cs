using Datory;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public class SignalRHubManager : Hub
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly ICacheManager _cacheManager;
        private readonly IUserRepository _userRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IExamPkRoomRepository _examPkRoomRepository;
        private readonly IExamPkRepository _examPkRepository;
        public SignalRHubManager(ISettingsManager settingsManager, IAuthManager authManager, IConfigRepository configRepository, ICacheManager cacheManager, IUserRepository userRepository, IExamPkRoomRepository examPkRoomRepository, IExamPkRepository examPkRepository)
        {
            _settingsManager = settingsManager;
            _configRepository = configRepository;
            _authManager = authManager;
            _cacheManager = cacheManager;
            _userRepository = userRepository;
            _examPkRoomRepository = examPkRoomRepository;
            _examPkRepository = examPkRepository;
        }
        /// <summary>
        /// 连接成功时触发
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var user = await _authManager.GetUserAsync();
            if (user != null && !_settingsManager.IsSafeMode)
            {
                var room = await _examPkRoomRepository.GetAsync(user.PkRoomId);

                var pk = await _examPkRepository.GetAsync(room.PkId);


                var userA = await _userRepository.GetByUserIdAsync(room.UserId_A);
                var userB = await _userRepository.GetByUserIdAsync(room.UserId_B);

                var cacheKeyPkRoom = CacheUtils.GetEntityKey("pkroom", room.Id);
                var cacheRoom = _cacheManager.Get<PkRoomResult>(cacheKeyPkRoom);

                if (cacheRoom == null)
                {
                    cacheRoom = new PkRoomResult
                    {

                        RoomId = room.Id,
                        RoomName = pk.Mark,

                        AUserId = userA.Id,
                        AUserDisplayName = userA.DisplayName,
                        AUserAvatarUrl = userA.AvatarbgUrl,
                        AUserState = PkRoomUserState.OffLine,
                        AUserStateStr = PkRoomUserState.OffLine.GetDisplayName(),

                        BUserId = userB.Id,
                        BUserDisplayName = userB.DisplayName,
                        BUserAvatarUrl = userB.AvatarbgUrl,
                        BUserState = PkRoomUserState.OffLine,
                        BUserStateStr = PkRoomUserState.OffLine.GetDisplayName(),

                        AnswerTmIds = new List<int>(),
                        AAnswers = new List<string>(),
                        BAnswers = new List<string>(),


                    };
                }

                if (user.Id == userA.Id)
                {
                    cacheRoom.AUserState = PkRoomUserState.OnLine;
                    cacheRoom.AUserStateStr = PkRoomUserState.OnLine.GetDisplayName();
                }
                if (user.Id == userB.Id)
                {
                    cacheRoom.BUserState = PkRoomUserState.OnLine;
                    cacheRoom.BUserStateStr = PkRoomUserState.OnLine.GetDisplayName();
                }

                cacheRoom.OnlineUserTotal++;

                _cacheManager.AddOrUpdateAbsolute(cacheKeyPkRoom, cacheRoom, 10);

                await Clients.All.SendAsync($"pkroom{cacheRoom.RoomId}", TranslateUtils.JsonSerialize(cacheRoom));
            }


        }
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var user = await _authManager.GetUserAsync();
            if (user != null && user.PkRoomId > 0)
            {
                var cacheKeyPkRoom = CacheUtils.GetEntityKey("pkroom", user.PkRoomId);
                var cacheRoom = _cacheManager.Get<PkRoomResult>(cacheKeyPkRoom);
                if (cacheRoom != null)
                {

                    if (user.Id == cacheRoom.AUserId)
                    {
                        cacheRoom.AUserState = cacheRoom.BUserState = PkRoomUserState.OnLine;
                        cacheRoom.AUserState = PkRoomUserState.OffLine;
                        cacheRoom.TmIndex = 0;

                    }
                    if (user.Id == cacheRoom.BUserId)
                    {
                        cacheRoom.AUserState = cacheRoom.BUserState = PkRoomUserState.OnLine;
                        cacheRoom.BUserState = PkRoomUserState.OffLine;
                        cacheRoom.TmIndex = 0;

                    }

                    cacheRoom.AUserStateStr = cacheRoom.AUserState.GetDisplayName();
                    cacheRoom.BUserStateStr = cacheRoom.BUserState.GetDisplayName();

                    cacheRoom.OnlineUserTotal--;

                    _cacheManager.AddOrUpdateAbsolute(cacheKeyPkRoom, cacheRoom, 10);

                    await Clients.All.SendAsync($"pkroom{cacheRoom.RoomId}", TranslateUtils.JsonSerialize(cacheRoom));
                }

                user.PkRoomId = 0;
                await _userRepository.UpdateByPkRoomAsync(user);
            }
        }

    }
}
