using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Core.Utils.Office;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseFilesController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(Route)]
        public async Task<ActionResult<GetResult>> Create([FromForm] CreateRequest request, [FromForm] IFormFile file)
        {
            if (_settingsManager.IsSafeMode)
            {
                return this.Error(Constants.ErrorSafe);
            }

            try
            {
                var adminAuth = await _authManager.GetAdminAuth();
                var admin = adminAuth.Admin;

                var fileName = PathUtils.GetFileName(file.FileName);

                var fileType = PathUtils.GetExtension(fileName);
                var config = await _configRepository.GetAsync();

                if (!FileUtils.IsPlayer(fileType) && !FileUtils.IsPDF(fileType) && !FileUtils.IsPPT(fileType) && !FileUtils.IsWord(fileType) && !FileUtils.IsExcel(fileType))
                {
                    return this.Error(Constants.ErrorUpload);
                }

                var realFileName = PathUtils.GetFileNameWithoutExtension(fileName);
                if (await _studyCourseFilesRepository.IsExistsAsync(realFileName, adminAuth.CurCompanyId, request.GroupId))
                {
                    return this.Error("文件已存在，请勿重复上传");
                }

                var path = _pathManager.GetCourseFilesUploadPath(adminAuth.CurCompanyId.ToString());
                if (request.GroupId > 0)
                {
                    var parentIds = await _studyCourseFilesGroupRepository.GetParentIdListAsync(request.GroupId);
                    if (parentIds != null && parentIds.Count > 0)
                    {
                        foreach (var id in parentIds)
                        {
                            var parentGroup = await _studyCourseFilesGroupRepository.GetAsync(id);
                            if (parentGroup != null)
                            {
                                path = PathUtils.Combine(path, parentGroup.GroupName);
                            }
                        }
                    }
                }
                var filePath = PathUtils.Combine(path, fileName);

                await _pathManager.UploadAsync(file, filePath);

                var coverurl = string.Empty;
                var duration = 0;
                if (FileUtils.IsPlayer(fileType))
                {
                    try
                    {
                        //cover
                        string base64String = request.Cover;
                        if (base64String.Contains("base64,"))
                        {
                            base64String = base64String.Split("base64,")[1];
                        }
                        string outputPath = PathUtils.Combine(path, $"{realFileName}_cover.jpg");
                        byte[] imageBytes = Convert.FromBase64String(base64String);
                        ImageUtils.Save(imageBytes, outputPath);
                        coverurl = _pathManager.GetRootUrlByPath(outputPath);
                    }
                    catch { }

                    duration = request.Duration;
                }

                var firstImgPath = PathUtils.Combine(path, $"{StringUtils.GetShortGuid()}.jpg");

                if (FileUtils.IsPDF(fileType))
                {
                    duration = AsposePdfObject.GetFirstImg(filePath, firstImgPath);
                }
                if (FileUtils.IsPPT(fileType))
                {
                    var (pdfpath, success, error) = AsposePptObject.GetPdfUrl(filePath);
                    if (success)
                    {
                        filePath = pdfpath;
                        duration = AsposePdfObject.GetFirstImg(pdfpath, firstImgPath);
                    }
                    else
                    {
                        return new GetResult { Success = false, Msg = error };
                    }
                }
                if (FileUtils.IsWord(fileType))
                {
                    var (pdfpath, success, error) = AsposeWordObject.GetPdfUrl(filePath);
                    if (success)
                    {
                        filePath = pdfpath;
                        duration = AsposePdfObject.GetFirstImg(pdfpath, firstImgPath);
                    }
                    else
                    {
                        return new GetResult { Success = false, Msg = error };
                    }
                }
                if (FileUtils.IsExcel(fileType))
                {
                    var (pdfpath, success, error) = AsposeExcelObject.GetPdfUrl(filePath);
                    if (success)
                    {
                        filePath = pdfpath;
                        duration = AsposePdfObject.GetFirstImg(pdfpath, firstImgPath);
                    }
                    else
                    {
                        return new GetResult { Success = false, Msg = error };
                    }
                }

                var firstImgUrl = _pathManager.DefaultBookCoverUrl;
                if (!string.IsNullOrEmpty(firstImgPath) && !FileUtils.IsPlayer(fileType))
                {
                    coverurl = _pathManager.GetRootUrlByPath(firstImgPath);
                }

                var url = _pathManager.GetRootUrlByPath(filePath);

                var courseFile = new StudyCourseFiles
                {
                    GroupId = request.GroupId,
                    FileName = realFileName,
                    FileType = fileType.ToUpper().Remove(0, 1),
                    Url = url,
                    CoverUrl = coverurl,
                    FileSize = TranslateUtils.ToInt(file.Length.ToString()),
                    Duration = duration,
                    CompanyId = adminAuth.CurCompanyId,
                    DepartmentId = admin.DepartmentId,
                    CreatorId = admin.Id,
                    CompanyParentPath = adminAuth.CompanyParentPath,
                    DepartmentParentPath = admin.DepartmentParentPath,
                };
                var fileId = await _studyCourseFilesRepository.InsertAsync(courseFile);

                await _authManager.AddAdminLogAsync("上传课件", courseFile.FileName);
                await _authManager.AddStatLogAsync(StatType.StudyFileAdd, "上传课件", fileId, courseFile.FileName);
                await _authManager.AddStatCount(StatType.StudyFileAdd);

                return new GetResult { Success = true };
            }
            catch (Exception ex)
            {
                return this.Error(ex.Message);
            }

        }
    }
}
