using Datory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class DashboardAdminController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetLogResult>> GetLog([FromQuery] GetLogRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var (total, list) = await _statLogRepository.GetListAsync(adminAuth, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var color = "success";
                    if (item.StatType.GetValue().Contains("Update"))
                    {
                        color = "warning";
                    }
                    if (item.StatType.GetValue().Contains("Delete"))
                    {
                        color = "danger";
                    }
                    if (item.StatType.GetValue().Contains("Export"))
                    {
                        color = "primary";
                        try
                        {
                            var entity = TranslateUtils.JsonDeserialize<StringResult>(item.LastEntity);
                            item.Set("Url", entity.Value);
                        }
                        catch
                        {
                            item.Set("Url", "");
                        }
                    }
                    item.Set("Color", color);

                    var adminName = "我";
                    if (item.AdminId != adminAuth.AdminId)
                    {
                        var otherAdmin = await _administratorRepository.GetByUserIdAsync(item.AdminId);
                        if (otherAdmin != null)
                        {
                            adminName = otherAdmin.DisplayName;
                        }
                    }

                    item.Set("AdminName", adminName);

                    item.Set("Title", item.StatTypeStr);

                    item.Set("Date", DateUtils.ParseThisMoment(item.CreatedDate.Value, DateTime.Now));

                    var name = item.ObjectName;


                    var isView = false;
                    var isEdit = false;


                    var isGift = false;
                    if (item.StatType == StatType.GiftAdd || item.StatType == StatType.GiftUpdate)
                    {
                        isGift = true;
                        if (await _databaseManager.PointShopRepository.ExistsAsync(item.ObjectId))
                        {
                            isEdit = true;
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsGift", isGift);

                    var isPlan = false;
                    if (item.StatType == StatType.StudyPlanAdd || item.StatType == StatType.StudyPlanUpdate)
                    {
                        isPlan = true;
                        if (await _databaseManager.StudyPlanRepository.ExistsAsync(item.ObjectId))
                        {
                            isView = true;
                            isEdit = true;
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsPlan", isPlan);

                    var isCourse = false;
                    var isFace = false;
                    if (item.StatType == StatType.StudyCourseAdd || item.StatType == StatType.StudyCourseUpdate)
                    {
                        isCourse = true;
                        if (await _databaseManager.StudyCourseRepository.ExistsAsync(item.ObjectId))
                        {
                            var course = await _databaseManager.StudyCourseRepository.GetAsync(item.ObjectId);
                            isFace = course.OffLine;
                            isView = true;
                            isEdit = true;
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsFace", isFace);
                    item.Set("IsCourse", isCourse);

                    var isFile = false;
                    if (item.StatType == StatType.StudyFileAdd || item.StatType == StatType.StudyFileUpdate)
                    {
                        isFile = true;
                        if (await _databaseManager.StudyCourseFilesRepository.ExistsAsync(item.ObjectId))
                        {
                            isView = true;
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsFile", isFile);

                    var isTm = false;
                    if (item.StatType == StatType.ExamTmAdd || item.StatType == StatType.ExamTmUpdate)
                    {
                        isTm = true;
                        if (await _databaseManager.ExamTmRepository.ExistsAsync(item.ObjectId))
                        {
                            isView = true;
                            isEdit = true;
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsTm", isTm);

                    var isDeleteTm = false;
                    if (item.StatType == StatType.ExamTmDelete)
                    {
                        isDeleteTm = true;
                        isView = true;

                    }
                    item.Set("IsDeleteTm", isDeleteTm);

                    var isUser = false;
                    if (item.StatType == StatType.UserAdd || item.StatType == StatType.UserUpdate)
                    {
                        isUser = true;
                        if (await _databaseManager.UserRepository.ExistsAsync(item.ObjectId))
                        {
                            isView = true;
                            isEdit = true;
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsUser", isUser);

                    var isAdmin = false;
                    if (item.StatType == StatType.AdminAdd || item.StatType == StatType.AdminUpdate)
                    {
                        isAdmin = true;
                        if (await _databaseManager.AdministratorRepository.ExistsAsync(item.ObjectId))
                        {
                            isView = true;
                            isEdit = true;
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsAdmin", isAdmin);

                    var isExam = false;
                    if (item.StatType == StatType.ExamAdd || item.StatType == StatType.ExamUpdate)
                    {
                        isExam = true;
                        var examPaper = await _databaseManager.ExamPaperRepository.GetAsync(item.ObjectId);
                        if (examPaper != null)
                        {
                            isEdit = true;
                            if (examPaper.TmRandomType != ExamPaperTmRandomType.RandomExaming && examPaper.SubmitType == SubmitType.Submit)
                            {
                                isView = true;
                            }
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsExam", isExam);

                    var isExamQ = false;
                    if (item.StatType == StatType.ExamQAdd || item.StatType == StatType.ExamQUpdate)
                    {
                        isExamQ = true;
                        if (await _databaseManager.ExamQuestionnaireRepository.ExistsAsync(item.ObjectId))
                        {
                            isView = true;
                            isEdit = true;
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsExamQ", isExamQ);

                    var isExamAss = false;
                    if (item.StatType == StatType.ExamAssAdd || item.StatType == StatType.ExamAssUpdate)
                    {
                        isExamAss = true;
                        if (await _databaseManager.ExamAssessmentRepository.ExistsAsync(item.ObjectId))
                        {
                            isView = true;
                            isEdit = true;
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsExamAss", isExamAss);

                    var isExamPk = false;
                    if (item.StatType == StatType.ExamPkAdd || item.StatType == StatType.ExamPkUpdate)
                    {
                        isExamPk = true;
                        if (await _databaseManager.ExamPkRepository.ExistsAsync(item.ObjectId))
                        {
                            isView = true;
                            isEdit = true;
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsExamPk", isExamPk);

                    var isExamCer = false;
                    if (item.StatType == StatType.ExamCerAdd || item.StatType == StatType.ExamCerUpdate)
                    {
                        isExamCer = true;
                        if (await _databaseManager.ExamCerRepository.ExistsAsync(item.ObjectId))
                        {
                            isView = true;
                            isEdit = true;
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsExamCer", isExamCer);

                    var isKnowledge = false;
                    if (item.StatType == StatType.KnowledgesAdd || item.StatType == StatType.KnowledgesUpdate)
                    {
                        isKnowledge = true;
                        if (await _databaseManager.KnowlegesRepository.ExistsAsync(item.ObjectId))
                        {
                            var knowledge = await _databaseManager.KnowlegesRepository.GetAsync(item.ObjectId);
                            item.Set("Src", knowledge.Url);
                            isView = true;
                        }
                        else
                        {
                            name = $"{name}(已删除)";
                        }
                    }
                    item.Set("IsKnowledge", isKnowledge);

                    var isTmCorrection = false;
                    if (item.StatType == StatType.ExamTmCorrectionAudit)
                    {
                        isTmCorrection = true;
                        isView = true;
                    }
                    item.Set("IsTmCorrection", isTmCorrection);

                    item.Set("Name", name);
                    item.Set("IsView", isView);
                    item.Set("IsEdit", isEdit);
                }
            }
            return new GetLogResult
            {
                Total = total,
                List = list
            };
        }


        [HttpGet, Route(RouteGetData)]
        public async Task<ActionResult<GetDataResult>> GetData()
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var config = await _configRepository.GetAsync();

            var o1 = 0; var o2 = 0; var o3 = 0; var o4 = 0; var o5 = 0;
            var admin1 = 0; var admin2 = 0; var admin3 = 0; var admin4 = 0; var admin5 = 0;
            var user1 = 0; var user2 = 0; var user3 = 0; var user4 = 0; var user5 = 0;


            var (c1, c2, c3, c4, c5) = await _databaseManager.OrganCompanyRepository.GetDataCount(adminAuth);
            var (d1, d2, d3, d4, d5) = await _databaseManager.OrganDepartmentRepository.GetDataCount(adminAuth);
            c3 = await _statRepository.SumAsync(StatType.CompanyDelete, adminAuth);
            d3 = await _statRepository.SumAsync(StatType.DepartmentDelete, adminAuth);
            o1 = c1 + d1;
            o3 = c3 + d3;
            o4 = c4 + d4;
            o5 = c5 + d5;
            o2 = o1 + o3;


            var (a1, a2, a3, a4, a5) = await _databaseManager.AdministratorRepository.GetDataCount(adminAuth);
            admin1 = a1; admin2 = a2; admin3 = a3; admin4 = a4; admin5 = a5;
            admin3 = await _statRepository.SumAsync(StatType.AdminDelete, adminAuth);
            admin2 = admin1 + admin3;

            var (u1, u2, u3, u4, u5) = await _databaseManager.UserRepository.GetDataCount(adminAuth);
            user1 = u1; user2 = u2; user3 = u3; user4 = u4; user5 = u5;
            user3 = await _statRepository.SumAsync(StatType.UserDelete, adminAuth);
            user2 = user1 + user3;


            var (e1, e2, e3, e4, e5) = await _databaseManager.ExamPaperRepository.GetDataCount(adminAuth);
            e3 = await _statRepository.SumAsync(StatType.ExamDelete, adminAuth);
            e2 = e1 + e3;

            var (m1, m2, m3, m4, m5) = await _databaseManager.ExamPaperRepository.GetDataCountMoni(adminAuth);

            var (q1, q2, q3, q4, q5) = await _databaseManager.ExamQuestionnaireRepository.GetDataCount(adminAuth);
            q3 = await _statRepository.SumAsync(StatType.ExamQDelete, adminAuth);
            q2 = q1 + q3;

            var (s1, s2, s3, s4, s5) = await _databaseManager.ExamAssessmentRepository.GetDataCount(adminAuth);
            s3 = await _statRepository.SumAsync(StatType.ExamAssDelete, adminAuth);
            s2 = s1 + s3;

            var (p1, p2, p3, p4, p5) = await _databaseManager.ExamPkRepository.GetDataCount(adminAuth);
            p3 = await _statRepository.SumAsync(StatType.ExamPkDelete, adminAuth);
            p2 = p1 + p3;

            var (cer1, cer2, cer3, cer4, cer5) = await _databaseManager.ExamCerRepository.GetDataCount(adminAuth);
            cer3 = await _statRepository.SumAsync(StatType.ExamCerDelete, adminAuth);
            cer2 = cer1 + cer3;

            var (t1, t2, t3, t4, t5) = await _databaseManager.ExamTmRepository.GetDataCount(adminAuth);
            t3 = await _statRepository.SumAsync(StatType.ExamTmDelete, adminAuth);
            t2 = t1 + t3;

            var (k1, k2, k3, k4, k5) = await _databaseManager.KnowlegesRepository.GetDataCount(adminAuth);
            k3 = await _statRepository.SumAsync(StatType.KnowledgesDelete, adminAuth);
            k2 = k1 + k3;


            var (plan1, plan2, plan3, plan4, plan5) = await _databaseManager.StudyPlanRepository.GetDataCount(adminAuth);
            plan3 = await _statRepository.SumAsync(StatType.StudyPlanDelete, adminAuth);
            plan2 = plan1 + plan3;

            var (course1, course2, course3, course4, course5) = await _databaseManager.StudyCourseRepository.GetDataCount(adminAuth);
            course3 = await _statRepository.SumAsync(StatType.StudyCourseDelete, adminAuth);
            course2 = course1 + course3;

            var (file1, file2, file3, file4, file5) = await _databaseManager.StudyCourseFilesRepository.GetDataCount(adminAuth);
            file3 = await _statRepository.SumAsync(StatType.StudyFileDelete, adminAuth);
            file2 = file1 + file3;

            var (gift1, gift2, gift3, gift4, gift5) = await _databaseManager.PointShopRepository.GetDataCount(adminAuth);
            gift3 = await _statRepository.SumAsync(StatType.GiftDelete, adminAuth);
            gift2 = gift1 + gift3;

            var dataList = new List<GetDataInfo>
                {
                    new() {
                        Name = "全部",
                        Data =config.SystemCode==SystemCode.Exam? [o1, admin1, user1,file1, t1, cer1, e1, m1, q1, s1, p1,k1,gift1]:[o1, admin1, user1,plan1,course1,file1, t1, cer1, e1, m1, q1, s1, p1,k1,gift1]
                    },
                    new() {
                        Name = "新增",
                        Data = config.SystemCode==SystemCode.Exam?[o2, admin2, user2, file2, t2, cer2, e2, m2, q2, s2, p2,k2, gift2]:[o2, admin2, user2, plan2, course2, file2, t2, cer2, e2, m2, q2, s2, p2,k2, gift2]
                    },
                    new() {
                        Name = "删除",
                        Data = config.SystemCode==SystemCode.Exam?[o3, admin3, user3, file3, t3, cer3, e3, m3, q3, s3, p3,k3, gift3]:[o3, admin3, user3, plan3, course3, file3, t3, cer3, e3, m3, q3, s3, p3,k3, gift3]
                    },
                    new() {
                        Name = "停用",
                        Data = config.SystemCode==SystemCode.Exam?[o4, admin4, user4, file4, t4, cer4, e4, m4, q4, s4, p4,k4, gift4]:[o4, admin4, user4, plan4, course4, file4, t4, cer4, e4, m4, q4, s4, p4,k4, gift4]
                    },
                    new() {
                        Name = "启用",
                        Data =  config.SystemCode==SystemCode.Exam?[o5, admin5, user5, file5, t5, cer5, e5, m5, q5, s5, p5,k5, gift5]:[o5, admin5, user5, plan5, course5, file5, t5, cer5, e5, m5, q5, s5, p5,k5, gift5]
                    }
                };

            var dataTitleList = new List<string>() { "组织", "管理员账号", "用户账号", "学习任务", "课程", "课件", "题目", "证书", "考试", "模拟", "问卷调查", "测评", "竞赛", "知识库", "商品" };
            if (config.SystemCode == SystemCode.Exam)
            {
                dataTitleList = new List<string>() { "组织", "管理员账号", "用户账号", "文档", "题目", "证书", "考试", "模拟", "问卷调查", "测评", "竞赛", "知识库", "商品" };
            }

            var (todayTotal, tlist) = await _examPaperRepository.GetListByDateAsync(adminAuth, "today");
            var (weekTotal, twlist) = await _examPaperRepository.GetListByDateAsync(adminAuth, "week");

            var (planMonthCreateTotal, planMonthCreateList) = await _databaseManager.StudyPlanRepository.GetListByCreateMAsync(adminAuth);
            var (planMonthOverTotal, planMonthOverList) = await _databaseManager.StudyPlanRepository.GetListByOverMAsync(adminAuth);

            var (offTrainTotal, offTrainList) = await _databaseManager.StudyCourseRepository.GetOffTrinListByWeekAsync(adminAuth);
            var (planoffTrainTotal, planoffTrainList) = await _databaseManager.StudyPlanCourseRepository.GetOffTrinListByWeekAsync(adminAuth);
            if (planoffTrainTotal > 0)
            {
                DateTime today = DateTime.Now;
                DateTime startOfWeek = today;
                DateTime endOfWeek = today;
                int dayOfWeek = (int)today.DayOfWeek;
                if (dayOfWeek == 0)
                {
                    startOfWeek = today.AddDays(-6);
                }
                else
                {
                    startOfWeek = today.AddDays(1 - dayOfWeek);
                }
                endOfWeek = startOfWeek.AddDays(6);
                foreach (var item in planoffTrainList)
                {
                    var offTrain = await _databaseManager.StudyCourseRepository.GetAsync(item.CourseId);
                    if (offTrain != null && offTrain.OfflineBeginDateTime.Value >= startOfWeek && offTrain.OfflineBeginDateTime <= endOfWeek)
                    {
                        offTrainTotal++;
                    }
                }
            }

            return new GetDataResult
            {
                DataList = dataList,
                DataTitleList = dataTitleList,
                ExamTotalToday = todayTotal,
                ExamTotalWeek = weekTotal,
                PlanCreateTotal = planMonthCreateTotal,
                PlanOverTotal = planMonthOverTotal,
                OffTrainTotal = offTrainTotal,
                SystemCode = config.SystemCode
            };
        }
    }
}
