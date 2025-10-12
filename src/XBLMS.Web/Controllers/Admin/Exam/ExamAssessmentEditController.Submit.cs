﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamAssessmentEditController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] GetSubmitRequest request)
        {
            if (request.Item.Id > 0)
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
                {
                    return this.NoAuth();
                }
            }
            else
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
                {
                    return this.NoAuth();
                }
            }

            var adminAuth = await _authManager.GetAdminAuth();
            var admin = adminAuth.Admin;
            var assInfo = request.Item;


            if (assInfo.Id > 0)
            {
                var last = await _examAssessmentRepository.GetAsync(assInfo.Id);

                if (request.SubmitType == SubmitType.Submit)
                {
                    assInfo.SubmitType = request.SubmitType;

                    await _examManager.ClearExamAssessment(assInfo.Id);
                    await _examManager.SerExamAssessmentTm(request.TmList, assInfo.Id);

                    _examManager.ArrangeAssessmentTask(assInfo.Id);

                    await _authManager.AddAdminLogAsync("重新发布测评", $"{assInfo.Title}");
                    await _authManager.AddStatLogAsync(StatType.ExamAssUpdate, "重新发布测评", assInfo.Id, assInfo.Title, last);


                }
                else
                {
                    await _authManager.AddAdminLogAsync("修改测评", $"{assInfo.Title}");
                    await _authManager.AddStatLogAsync(StatType.ExamAssUpdate, "修改测评", assInfo.Id, assInfo.Title, last);
                }


                await _examAssessmentUserRepository.UpdateKeyWordsAsync(assInfo.Id, assInfo.Title);
                await _examAssessmentUserRepository.UpdateExamDateTimeAsync(assInfo.Id, assInfo.ExamBeginDateTime.Value, assInfo.ExamEndDateTime.Value);


                await _examAssessmentRepository.UpdateAsync(assInfo);

            }
            else
            {
                assInfo.SubmitType = request.SubmitType;
                assInfo.CompanyId = adminAuth.CurCompanyId; 
                assInfo.CreatorId = admin.Id;
                assInfo.DepartmentId = admin.DepartmentId;
                assInfo.CompanyParentPath = adminAuth.CompanyParentPath;
                assInfo.DepartmentParentPath = admin.DepartmentParentPath;

                var paperId = await _examAssessmentRepository.InsertAsync(assInfo);
                assInfo.Id = paperId;

                await _examManager.SerExamAssessmentTm(request.TmList, paperId);


                if (request.SubmitType == SubmitType.Submit)
                {
                    _examManager.ArrangeAssessmentTask(assInfo.Id);

                    await _authManager.AddAdminLogAsync("发布测评", $"{assInfo.Title}");
                    await _authManager.AddStatLogAsync(StatType.ExamAssAdd, "发布测评", assInfo.Id, assInfo.Title);
                    await _authManager.AddStatCount(StatType.ExamAssAdd);
                }
                else
                {
                    await _authManager.AddAdminLogAsync("保存测评", $"{assInfo.Title}");
                    await _authManager.AddStatLogAsync(StatType.ExamAssAdd, "保存测评", assInfo.Id, assInfo.Title);
                    await _authManager.AddStatCount(StatType.ExamAssAdd);
                }
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
