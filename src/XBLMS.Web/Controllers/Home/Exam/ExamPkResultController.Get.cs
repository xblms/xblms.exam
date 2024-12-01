using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPkResultController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();

            var room = await _examPkRoomRepository.GetAsync(request.Id);

            if (user.Id == room.UserId_A && !room.Finished)
            {
                var cacheKeyPkRoom = CacheUtils.GetEntityKey("pkroom", room.Id);
                var cacheRoom = _cacheManager.Get<PkRoomResult>(cacheKeyPkRoom);

                room.AnswerTotal_A = room.AnswerTotal_B = cacheRoom.AnswerTotal;
                room.Finished = true;
                room.RightTotal_A = cacheRoom.AUserAnswerRightTotal;
                room.RightTotal_B = cacheRoom.BUserAnswerRightTotal;
                room.DurationTotal_A = cacheRoom.AUserDuration;
                room.DurationTotal_B = cacheRoom.BUserDuration;
                room.AAnswers = cacheRoom.AAnswers;
                room.BAnswers = cacheRoom.BAnswers;
                room.AnswerTmIds = cacheRoom.AnswerTmIds;

                var failUserid = 0;

                if (cacheRoom.AUserAnswerRightTotal > cacheRoom.BUserAnswerRightTotal)
                {
                    room.WinUserId = room.UserId_A;
                    room.State_A = PkRoomUserState.Win;
                    room.State_B = PkRoomUserState.Fail;
                }
                else if (cacheRoom.AUserAnswerRightTotal == cacheRoom.BUserAnswerRightTotal)
                {
                    if (cacheRoom.AUserDuration > cacheRoom.BUserDuration)
                    {
                        room.WinUserId = room.UserId_B;
                        room.State_A = PkRoomUserState.Fail;
                        room.State_B = PkRoomUserState.Win;

                        failUserid = room.UserId_A;
                    }
                    else
                    {
                        room.WinUserId = room.UserId_A;
                        room.State_A = PkRoomUserState.Win;
                        room.State_B = PkRoomUserState.Fail;

                        failUserid = room.UserId_B;
                    }
                }
                else
                {
                    room.WinUserId = room.UserId_B;
                    room.State_A = PkRoomUserState.Fail;
                    room.State_B = PkRoomUserState.Win;

                    failUserid = room.UserId_A;
                }



                await _examPkRoomRepository.UpdateAsync(room);
             

                var pk = await _examPkRepository.GetAsync(room.PkId);
                if (pk != null)
                {
                    if (pk.Vs > 4)
                    {
                        var nextPk = await _examPkRepository.GetNextAsync(pk.ParentId, pk.Current + 1);
                        if (nextPk != null)
                        {
                            await _examPkRoomRepository.RandomPositonAsync(nextPk.Id, room.WinUserId);
                        }
                    }
                    if (pk.Vs == 4)
                    {
                        var three = await _examPkRepository.GetNextAsync(pk.ParentId, pk.Current + 1, 3);
                        var two = await _examPkRepository.GetNextAsync(pk.ParentId, pk.Current + 1, 2);

                        if (three != null)
                        {
                            await _examPkRoomRepository.RandomPositonAsync(three.Id, failUserid);
                        }

                        if (two != null)
                        {
                            await _examPkRoomRepository.RandomPositonAsync(three.Id, room.WinUserId);
                        }
                    }

                    var pkusera = await _examPkUserRepository.GetAsync(pk.ParentId, room.UserId_A);
                    pkusera.AnswerTotal += room.AnswerTotal_A;
                    pkusera.RightTotal += room.RightTotal_A;
                    pkusera.DurationTotal += room.DurationTotal_A;
                    await _examPkUserRepository.UpdateAsync(pkusera);

                    var pkuserb = await _examPkUserRepository.GetAsync(pk.ParentId, room.UserId_B);
                    pkuserb.AnswerTotal += room.AnswerTotal_B;
                    pkuserb.RightTotal += room.RightTotal_B;
                    pkuserb.DurationTotal += room.DurationTotal_B;
                    await _examPkUserRepository.UpdateAsync(pkuserb);
                }


                _cacheManager.Remove(cacheKeyPkRoom);
            }

            Thread.Sleep(3000);
            room = await _examPkRoomRepository.GetAsync(request.Id);
            var usera = await _organManager.GetUser(room.UserId_A);
            room.Set("UserA", usera);

            var userb = await _organManager.GetUser(room.UserId_B);
            room.Set("UserB", userb);

            var pkInfo = await _examPkRepository.GetAsync(room.PkId);

            return new GetResult
            {
                Room = room,
                Title = $"{pkInfo.Name}（{pkInfo.Mark}）"
            };
        }
    }
}
