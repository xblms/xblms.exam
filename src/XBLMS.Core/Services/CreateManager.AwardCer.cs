using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class CreateManager
    {
        public async Task AwardCer(ExamPaper paper, int startId, int userId)
        {
            var examStar = await _databaseManager.ExamPaperStartRepository.GetAsync(startId);
            var user = await _databaseManager.UserRepository.GetByUserIdAsync(userId);
            if (user != null && paper != null)
            {
                if (paper.CerId > 0)
                {
                    var cerInfo = await _databaseManager.ExamCerRepository.GetAsync(paper.CerId);
                    if (cerInfo != null)
                    {
                        if (!await _databaseManager.ExamCerUserRepository.ExistsAsync(userId, paper.Id))
                        {
                            var cerId = await _databaseManager.ExamCerUserRepository.InsertAsync(new ExamCerUser
                            {
                                CompanyId = user.CompanyId,
                                DepartmentId = user.DepartmentId,
                                CreatorId = user.Id,
                                UserId = user.Id,
                                CerId = paper.CerId,
                                ExamPaperId = paper.Id,
                                ExamStartId = startId,
                                CerNumber = GetCertificateNumber(cerInfo.Prefix),
                                CerDateTime = DateTime.Now,
                                KeyWords = paper.Title,
                                KeyWordsAdmin = await _organManager.GetUserKeyWords(userId)

                            });
                            if (cerId > 0)
                            {
                                var cerUser = await _databaseManager.ExamCerUserRepository.GetAsync(cerId);
                                await AwardCerImg(paper, cerInfo, cerUser, user, examStar.Score);
                            }
                        }
                    }
                }
            }
        }
        private async Task AwardCerImg(ExamPaper paper, ExamCer cerInfo, ExamCerUser cerUser, User user, decimal score)
        {
            if (string.IsNullOrWhiteSpace(cerUser.CerImg))
            {
                if (!string.IsNullOrWhiteSpace(cerInfo.BackgroundImg))
                {
                    var oldPath = PathUtils.Combine(_settingsManager.WebRootPath, cerInfo.BackgroundImg);
                    var fileName = $"{cerUser.CerNumber}.jpg";
                    var filePath = _pathManager.GetCerUploadFilesPath(fileName);
                    FileUtils.DeleteFileIfExists(filePath);
                    FileUtils.CopyFile(oldPath, filePath);
                    var fontSize = cerInfo.FontSize;

                    var positionmodel = TranslateUtils.JsonDeserialize<List<ExamCertPosition>>(cerInfo.Get("position").ToString());

                    foreach (var position in positionmodel)
                    {
                        //1001 姓名 2暂无 3暂无 4证件照  5证书编号 6认证日期 7颁发单位 10试卷 11 成绩
                        if (position.Id == "1004")
                        {
                            var avatar = user.AvatarCerUrl;
                            if (string.IsNullOrWhiteSpace(avatar))
                            {
                                avatar = _pathManager.DefaultAvatarUrl;
                            }
                            var waterImg = PathUtils.Combine(_settingsManager.WebRootPath, avatar);
                            var waterImgFileName = PathUtils.GetFileName(waterImg);
                            var waterImgNewfilepath = _pathManager.GetCerUploadFilesPath(waterImgFileName);
                            if (!FileUtils.IsFileExists(waterImgNewfilepath))
                            {
                                ImageUtils.MakeThumbnail(waterImg, waterImgNewfilepath, position.Width, position.Height, true);
                            }
                            _pathManager.AddImageMarkForCertificateReviewAsync(filePath, waterImgNewfilepath, position.PageX, position.PageY, position.Width, position.Height);
                        }
                        else
                        {
                            if (position.Id == "1001")
                            {
                                _pathManager.AddWaterMarkForCertificateReviewAsync(filePath, user.DisplayName, fontSize, position.PageX, position.PageY);
                            }

                            if (position.Id == "1005")
                            {
                                _pathManager.AddWaterMarkForCertificateReviewAsync(filePath, cerUser.CerNumber, fontSize, position.PageX, position.PageY);
                            }
                            if (position.Id == "1006")
                            {
                                _pathManager.AddWaterMarkForCertificateReviewAsync(filePath, DateUtils.GetDateString(cerUser.CerDateTime.Value), fontSize, position.PageX, position.PageY);
                            }
                            if (position.Id == "1007")
                            {
                                _pathManager.AddWaterMarkForCertificateReviewAsync(filePath, cerInfo.OrganName, fontSize, position.PageX, position.PageY);
                            }
                            if (position.Id == "1010")//试卷
                            {
                                _pathManager.AddWaterMarkForCertificateReviewAsync(filePath, paper.Title, fontSize, position.PageX, position.PageY);
                            }
                            if (position.Id == "1011")//成绩
                            {
                                _pathManager.AddWaterMarkForCertificateReviewAsync(filePath, score.ToString(), fontSize, position.PageX, position.PageY);
                            }
                        }
                    }
                    var cerImg = _pathManager.GetRootUrlByPath(filePath);
                    await _databaseManager.ExamCerUserRepository.UpdateImgAsync(cerUser.Id, cerImg);
                }
            }
        }
        private static string GetCertificateNumber(string prefix)
        {
            return $"{prefix}-{DateUtils.GetUnixTimestamp(DateTime.Now)}";
        }
    }
}
