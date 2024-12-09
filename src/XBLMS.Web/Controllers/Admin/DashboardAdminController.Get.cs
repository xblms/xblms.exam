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
            var admin = await _authManager.GetAdminAsync();
            var adminId = admin.Id;
            if (admin.Auth == AuthorityType.Admin)
            {
                adminId = 0;
            }
            var (total, list) = await _statLogRepository.GetListAsync(null, null, adminId, request.PageIndex, request.PageSize);
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
                        var entity = TranslateUtils.JsonDeserialize<StringResult>(item.LastEntity);
                        item.Set("Url", entity.Value);
                    }
                    item.Set("Color", color);

                    var adminName = "我";
                    if (item.AdminId != admin.Id)
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
                        if (await _databaseManager.ExamPaperRepository.ExistsAsync(item.ObjectId))
                        {
                            isView = true;
                            isEdit = true;
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
            var admin = await _authManager.GetAdminAsync();


            var (c1, c2, c3, c4, c5) = await _databaseManager.OrganCompanyRepository.GetDataCount();
            var (d1, d2, d3, d4, d5) = await _databaseManager.OrganDepartmentRepository.GetDataCount();
            var (g1, g2, g3, g4, g5) = await _databaseManager.OrganDutyRepository.GetDataCount();
            c3 = await _statRepository.SumAsync(StatType.CompanyDelete);
            d3 = await _statRepository.SumAsync(StatType.DepartmentDelete);
            g3 = await _statRepository.SumAsync(StatType.DutyDelete);
            var o1 = c1 + d1 + g1;
            var o3 = c3 + d3 + g3;
            var o4 = c4 + d4 + g4;
            var o5 = c5 + d5 + g5;
            var o2 = o1 + o3;



            var (admin1, admin2, admin3, admin4, admin5) = await _databaseManager.AdministratorRepository.GetDataCount();
            admin3 = await _statRepository.SumAsync(StatType.AdminDelete);
            admin2 = admin1 + admin3;

            var (user1, user2, user3, user4, user5) = await _databaseManager.UserRepository.GetDataCount();
            user3 = await _statRepository.SumAsync(StatType.UserDelete);
            user2 = user1 + user3;

            var (e1, e2, e3, e4, e5) = await _databaseManager.ExamPaperRepository.GetDataCount();
            e3 = await _statRepository.SumAsync(StatType.ExamDelete);
            e2 = e1 + e3;

            var (m1, m2, m3, m4, m5) = await _databaseManager.ExamPaperRepository.GetDataCountMoni();

            var (q1, q2, q3, q4, q5) = await _databaseManager.ExamQuestionnaireRepository.GetDataCount();
            q3 = await _statRepository.SumAsync(StatType.ExamQDelete);
            q2 = q1 + q3;

            var (s1, s2, s3, s4, s5) = await _databaseManager.ExamAssessmentRepository.GetDataCount();
            s3 = await _statRepository.SumAsync(StatType.ExamAssDelete);
            s2 = s1 + s3;

            var (p1, p2, p3, p4, p5) = await _databaseManager.ExamPkRepository.GetDataCount();
            p3 = await _statRepository.SumAsync(StatType.ExamPkDelete);
            p2 = p1 + p3;

            var (cer1, cer2, cer3, cer4, cer5) = await _databaseManager.ExamCerRepository.GetDataCount();
            cer3 = await _statRepository.SumAsync(StatType.ExamCerDelete);
            cer2 = cer1 + cer3;

            var (t1, t2, t3, t4, t5) = await _databaseManager.ExamTmRepository.GetDataCount();
            t3 = await _statRepository.SumAsync(StatType.ExamTmDelete);
            t2 = t1 + t3;

            var dataList = new List<GetDataInfo>();
            dataList.Add(new GetDataInfo
            {
                Name = "全部",
                Data = [o1, admin1, user1, t1, cer1, e1, m1, q1, s1, p1]
            });
            dataList.Add(new GetDataInfo
            {
                Name = "新增",
                Data = [o2, admin2, user2, t2, cer2, e2, m2, q2, s2, p2]
            });
            dataList.Add(new GetDataInfo
            {
                Name = "删除",
                Data = [o3, admin3, user3, t3, cer3, e3, m3, q3, s3, p3]
            });
            dataList.Add(new GetDataInfo
            {
                Name = "停用",
                Data = new List<int> { o4, admin4, user4, t4, cer4, e4, m4, q4, s4, p4 }
            });
            dataList.Add(new GetDataInfo
            {
                Name = "启用",
                Data = [o5, admin5, user5, t5, cer5, e5, m5, q5, s5, p5]
            });

            return new GetDataResult
            {
                DataList = dataList,
                DataTitleList = new List<string> { "组织", "管理员账号", "用户账号", "题目", "证书", "考试", "模拟", "问卷调查", "测评", "竞赛" }
            };
        }
    }
}
