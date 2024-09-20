using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Models;
using XBLMS.Utils;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using System;
using XBLMS.Enums;
using System.Collections.Generic;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamCerEditController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<IntResult>> Submit([FromBody] ItemRequest<ExamCer> request)
        {
            var admin = await _authManager.GetAdminAsync();
            var cer = request.Item;
            if (cer.Id > 0)
            {
                var cerInfo = await _examCerRepository.GetAsync(cer.Id);
                await _examCerRepository.UpdateAsync(cer);
                await _authManager.AddAdminLogAsync("修改证书模板", $"名称:{cerInfo.Name}");
                return new IntResult
                {
                    Value = cerInfo.Id
                };
            }
            else
            {

                cer.CompanyId = admin.CompanyId;
                cer.CreatorId = admin.Id;
                cer.DepartmentId = admin.DepartmentId;
                var id = await _examCerRepository.InsertAsync(cer);
                await _statRepository.AddCountAsync(StatType.ExamCerAdd);
                await _authManager.AddAdminLogAsync("添加证书模板", $"名称:{cer.Name}");
                return new IntResult
                {
                    Value = id
                };
            }
        }

        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteSubmitPosition)]
        public async Task<ActionResult<BoolResult>> SubmitPosition([FromBody] SubmitWartmarkPositionData request)
        {
            var admin = await _authManager.GetAdminAsync();
 
            var cerInfo = await _examCerRepository.GetAsync(request.Id);
            if (!string.IsNullOrWhiteSpace(cerInfo.BackgroundImg))
            {
                var oldPath = PathUtils.Combine(_settingsManager.WebRootPath, cerInfo.BackgroundImg);
                var fileName = $"preview_cer_{PathUtils.GetFileName(cerInfo.BackgroundImg)}";
                var filePath = _pathManager.GetCerUploadFilesPath(fileName);
                FileUtils.DeleteFileIfExists(filePath);
                FileUtils.CopyFile(oldPath, filePath);
                var fontSize = cerInfo.FontSize;
                var positions = TranslateUtils.JsonDeserialize<List<ExamCertPosition>>(cerInfo.Get("position").ToString());


                foreach (var position in positions)
                {
                    if (position.Id == "1004")
                    {
                        var waterImg = PathUtils.Combine(_settingsManager.WebRootPath, _pathManager.DefaultAvatarUrl);
                        var waterImgFileName = _pathManager.GetUploadFileName(waterImg);
                        var waterImgNewfilepath = _pathManager.GetCerUploadFilesPath(waterImgFileName);
                        if (!FileUtils.IsFileExists(waterImgNewfilepath))
                        {
                            ImageUtils.MakeThumbnail(waterImg, waterImgNewfilepath, position.Width, position.Height, true);
                        }
                        _pathManager.AddImageMarkForCertificateReviewAsync(filePath, waterImgNewfilepath, position.PageX, position.PageY, position.Width, position.Height);
                    }
                    else
                    {
                        _pathManager.AddWaterMarkForCertificateReviewAsync(filePath, position.Text, Convert.ToInt32(fontSize), position.PageX, position.PageY);
                    }

                }
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
