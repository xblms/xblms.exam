using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseFilesSelectController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetQueryResult>> Get([FromQuery] GetRequest request)
        {
            var admin = await _authManager.GetAdminAsync();
            if (admin == null) return Unauthorized();


            var auth = await _authManager.GetAdminAuth();

            var idList = new List<int> { 0 };

            var resultList = new List<GetListInfo>();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                var files = await _studyCourseFilesRepository.GetAllAsync(auth, request.Keyword, request.FileType);

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
                            CoverView = await _pathManager.GetServerFileUrl(file.CoverUrl),
                            Url = file.Url,
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

                var files = await _studyCourseFilesRepository.GetAllAsync(auth, request.GroupId, request.FileType);

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
                            CoverView = await _pathManager.GetServerFileUrl(file.CoverUrl),
                            Url = file.Url
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
