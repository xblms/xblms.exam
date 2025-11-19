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
        private async Task ExecuteAwardCer(int taskId)
        {
            var examStart = await _databaseManager.ExamPaperStartRepository.GetAsync(taskId);
            var paper = await _databaseManager.ExamPaperRepository.GetAsync(examStart.ExamPaperId);
            var user = await _databaseManager.UserRepository.GetByUserIdAsync(examStart.UserId);
            if (user != null && paper != null)
            {
                if (paper.CerId > 0)
                {
                    var cerInfo = await _databaseManager.ExamCerRepository.GetAsync(paper.CerId);
                    if (cerInfo != null)
                    {
                        if (!await _databaseManager.ExamCerUserRepository.ExistsAsync(user.Id, paper.Id, examStart.PlanId, examStart.CourseId))
                        {
                            var cerId = await _databaseManager.ExamCerUserRepository.InsertAsync(new ExamCerUser
                            {
                                PlanId = examStart.PlanId,
                                CourseId = examStart.CourseId,
                                CompanyId = user.CompanyId,
                                DepartmentId = user.DepartmentId,
                                CreatorId = user.Id,
                                UserId = user.Id,
                                CerId = paper.CerId,
                                ExamPaperId = paper.Id,
                                ExamStartId = examStart.Id,
                                CerNumber = GetCertificateNumber(cerInfo.Prefix),
                                CerDateTime = DateTime.Now,
                                KeyWords = paper.Title,
                                KeyWordsAdmin = await _organManager.GetUserKeyWords(user.Id),
                                DepartmentParentPath = user.DepartmentParentPath,
                                CompanyParentPath = user.CompanyParentPath
                            });
                            if (cerId > 0)
                            {
                                var cerUser = await _databaseManager.ExamCerUserRepository.GetAsync(cerId);
                                await AwardCerImg(paper, cerInfo, cerUser, user, examStart.Score);
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
                        //1001 姓名 2暂无 3暂无 4证件照  5证书编号 6认证日期 7颁发单位 8计划 9课程 10试卷 11 成绩
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
                            if (position.Id == "1008")//计划
                            {
                                if (cerUser.PlanId > 0)
                                {
                                    var plan = await _databaseManager.StudyPlanRepository.GetAsync(cerUser.PlanId);
                                    if (plan != null)
                                    {
                                        _pathManager.AddWaterMarkForCertificateReviewAsync(filePath, plan.PlanName, fontSize, position.PageX, position.PageY);
                                    }
                                }
                            }
                            if (position.Id == "1009")//课程
                            {
                                if ((cerUser.CourseId > 0))
                                {
                                    var course = await _databaseManager.StudyCourseRepository.GetAsync(cerUser.CourseId);
                                    if (course != null)
                                    {
                                        if (cerUser.PlanId > 0)
                                        {
                                            var planCourse = await _databaseManager.StudyPlanCourseRepository.GetAsync(cerUser.PlanId, cerUser.CourseId);
                                            if (planCourse != null)
                                            {
                                                course.Name = planCourse.CourseName;
                                            }
                                        }
                                        _pathManager.AddWaterMarkForCertificateReviewAsync(filePath, course.Name, fontSize, position.PageX, position.PageY);
                                    }
                                }
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
