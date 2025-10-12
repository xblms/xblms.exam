using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireEditController
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
            var paper = request.Item;

            if (paper.IsCourseUse)
            {
                paper.Published = false;
            }

            if (paper.Id > 0)
            {
                var last = await _questionnaireRepository.GetAsync(paper.Id);

                if (request.SubmitType == SubmitType.Submit)
                {
                    paper.SubmitType = request.SubmitType;

                    await _examManager.ClearQuestionnaire(paper.Id);
                    await _examManager.SetQuestionnairTm(request.TmList, paper.Id);

                    _examManager.ArrangeQuestionnaireTask(paper.Id);

                    await _authManager.AddAdminLogAsync("重新发布调查问卷", $"{paper.Title}");
                    await _authManager.AddStatLogAsync(StatType.ExamQUpdate, "重新发布调查问卷", paper.Id, paper.Title, last);

                }
                else
                {
                    await _authManager.AddAdminLogAsync("修改调查问卷", $"{paper.Title}");
                    await _authManager.AddStatLogAsync(StatType.ExamQUpdate, "修改调查问卷", paper.Id, paper.Title, last);
                }



                if (!paper.Published)
                {
                    await _questionnaireUserRepository.UpdateKeyWordsAsync(paper.Id, paper.Title);
                    await _questionnaireUserRepository.UpdateExamDateTimeAsync(paper.Id, paper.ExamBeginDateTime.Value, paper.ExamEndDateTime.Value);
                }


                await _questionnaireRepository.UpdateAsync(paper);

            }
            else
            {
                paper.SubmitType = request.SubmitType;
                paper.CompanyId = adminAuth.CurCompanyId;
                paper.CreatorId = admin.Id;
                paper.DepartmentId = admin.DepartmentId;
                paper.CompanyParentPath = adminAuth.CompanyParentPath;
                paper.DepartmentParentPath = admin.DepartmentParentPath;

                var paperId = await _questionnaireRepository.InsertAsync(paper);
                paper.Id = paperId;

                await _examManager.SetQuestionnairTm(request.TmList, paperId);


                if (request.SubmitType == SubmitType.Submit)
                {
                    _examManager.ArrangeQuestionnaireTask(paper.Id);

                    await _authManager.AddAdminLogAsync("发布调查问卷", $"{paper.Title}");
                    await _authManager.AddStatLogAsync(StatType.ExamQAdd, "发布调查问卷", paper.Id, paper.Title);
                    await _authManager.AddStatCount(StatType.ExamQAdd);
                }
                else
                {
                    await _authManager.AddAdminLogAsync("保存调查问卷", $"{paper.Title}");
                    await _authManager.AddStatLogAsync(StatType.ExamQAdd, "保存调查问卷", paper.Id, paper.Title);
                    await _authManager.AddStatCount(StatType.ExamQAdd);
                }
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
