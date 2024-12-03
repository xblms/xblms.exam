using Microsoft.AspNetCore.Mvc;
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


            var admin = await _authManager.GetAdminAsync();
            var assInfo = request.Item;


            if (assInfo.Id > 0)
            {

                if (request.SubmitType == SubmitType.Submit)
                {
                    assInfo.SubmitType = request.SubmitType;

                    await _examManager.ClearExamAssessment(assInfo.Id);

                    await _examManager.ArrangerExamAssessment(assInfo);

                    await _examManager.SerExamAssessmentTm(request.TmList, assInfo.Id);

                    await _authManager.AddAdminLogAsync("重新发布测评", $"{assInfo.Title}");


                }
                else
                {
                    await _authManager.AddAdminLogAsync("修改测评", $"{assInfo.Title}");
                }


                await _examAssessmentUserRepository.UpdateKeyWordsAsync(assInfo.Id, assInfo.Title);
                await _examAssessmentUserRepository.UpdateExamDateTimeAsync(assInfo.Id, assInfo.ExamBeginDateTime.Value, assInfo.ExamEndDateTime.Value);


                await _examAssessmentRepository.UpdateAsync(assInfo);

            }
            else
            {
                assInfo.SubmitType = request.SubmitType;
                assInfo.CompanyId = admin.CompanyId;
                assInfo.CreatorId = admin.Id;
                assInfo.DepartmentId = admin.DepartmentId;

                var paperId = await _examAssessmentRepository.InsertAsync(assInfo);
                assInfo.Id = paperId;

                await _examManager.SerExamAssessmentTm(request.TmList, paperId);


                if (request.SubmitType == SubmitType.Submit)
                {
                    await _examManager.ArrangerExamAssessment(assInfo);

                    await _authManager.AddAdminLogAsync("发布测评", $"{assInfo.Title}");
                }
                else
                {
                    await _authManager.AddAdminLogAsync("保存测评", $"{assInfo.Title}");
                }
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
