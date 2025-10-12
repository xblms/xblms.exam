using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseFilesController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetQueryResult>> Get([FromQuery] GetRequest request)
        {

            if (!string.IsNullOrWhiteSpace(request.Token))
            {
                var token = _settingsManager.Decrypt(request.Token);
                var tokenInfo = TranslateUtils.JsonDeserialize<FileRedirectToken>(token);
                if (TranslateUtils.ToDateTime(tokenInfo.Timespan) > System.DateTime.Now)
                {
                    var userName = tokenInfo.UserName;
                    var tokenAdmin = await _administratorRepository.GetByAccountAsync(userName);
                    var logintoken = _authManager.AuthenticateAdministrator(tokenAdmin, true);

                    var cacheKey = Constants.GetSessionIdCacheKey(tokenAdmin.Id);
                    var sessionId = StringUtils.Guid();
                    await _dbCacheRepository.RemoveAndInsertAsync(cacheKey, sessionId);

                    return new GetQueryResult
                    {
                        SessionId = sessionId,
                        Token = logintoken
                    };
                }
                else
                {
                    return this.Error("授权过期");
                }
            }


            var admin = await _authManager.GetAdminAsync();
            if (admin == null) return Unauthorized();

            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }


            if (!request.IsFileServer)
            {
                var config = await _configRepository.GetAsync();
                if (config.BushuFilesServer)
                {
                    var token = _settingsManager.Encrypt(TranslateUtils.JsonSerialize(new FileRedirectToken
                    {
                        Timespan = DateTime.Now.AddHours(8).ToString(),
                        UserName = admin.UserName
                    }));
                    return new GetQueryResult
                    {
                        RedirectUrl = $"{config.BushuFilesServerUrl}/admin/study/studyCourseFiles?token={token}"
                    };
                }
            }


            var auth = await _authManager.GetAdminAuth();

            var idList = new List<int> { 0 };

            var resultList = new List<GetListInfo>();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                var files = await _studyCourseFilesRepository.GetAllAsync(auth, request.Keyword, string.Empty);

                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        resultList.Add(new GetListInfo
                        {
                            Id = file.Id,
                            Name = file.FileName,
                            FileType = file.FileType,
                            Type = "File",
                            Size = file.FileSize,
                            Duration = file.Duration,
                            DateTimeStr = file.LastModifiedDate.Value.ToString(DateUtils.FormatStringDateOnlyCN),
                            Cover = file.CoverUrl,
                            IsVideo = FileUtils.IsPlayer(file.FileType)
                        });
                    }
                }
            }
            else
            {
                var groups = await _studyCourseFilesGroupRepository.GetListAsync(auth, request.GroupId);

                if (groups != null && groups.Count > 0)
                {
                    foreach (var group in groups)
                    {
                        var childIds = await _studyCourseFilesGroupRepository.GetChildIdListAsync(group.Id);
                        var sumSize = await _studyCourseFilesRepository.SumFileSizeAsync(childIds);
                        resultList.Add(new GetListInfo
                        {
                            Id = group.Id,
                            Name = group.GroupName,
                            Type = "Group",
                            Size = sumSize,
                            DateTimeStr = group.LastModifiedDate.Value.ToString(DateUtils.FormatStringDateOnlyCN)
                        });
                    }
                }

                var files = await _studyCourseFilesRepository.GetAllAsync(auth, request.GroupId, string.Empty);

                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        resultList.Add(new GetListInfo
                        {
                            Id = file.Id,
                            Name = file.FileName,
                            FileType = file.FileType,
                            Type = "File",
                            Size = file.FileSize,
                            Duration = file.Duration,
                            DateTimeStr = file.LastModifiedDate.Value.ToString(DateUtils.FormatStringDateOnlyCN),
                            Cover = file.CoverUrl,
                            IsVideo = FileUtils.IsPlayer(file.FileType)
                        });
                    }
                }


                if (request.GroupId > 0)
                {
                    var parentIds = await _studyCourseFilesGroupRepository.GetParentIdListAsync(request.GroupId);
                    if (parentIds != null && parentIds.Count > 0)
                    {
                        idList.AddRange(parentIds);
                    }
                }
            }


            var pathList = new List<GetQueryResultPath> { };
            foreach (int id in idList)
            {
                var pathInfo = new GetQueryResultPath() { };
                if (id == 0)
                {
                    pathInfo.Id = 0;
                    pathInfo.Name = "根目录";
                }
                else
                {
                    var groupInfo = await _studyCourseFilesGroupRepository.GetAsync(id);
                    pathInfo.Id = groupInfo.Id;
                    pathInfo.Name = groupInfo.GroupName;
                }
                pathList.Add(pathInfo);
            }

            return new GetQueryResult
            {
                List = resultList,
                Paths = pathList
            };
        }
    }
}
