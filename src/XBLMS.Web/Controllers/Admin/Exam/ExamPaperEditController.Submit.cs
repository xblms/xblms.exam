using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperEditController
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

            var tree = await _examPaperTreeRepository.GetAsync(paper.TreeId);
            if (tree != null)
            {
                paper.TreeParentPath = tree.ParentPath;
            }

            if (paper.Id > 0)
            {
                var oldPaper = await _examPaperRepository.GetAsync(paper.Id);

                if (request.SubmitType == SubmitType.Submit)
                {
                    paper.SubmitType = request.SubmitType;

                    await _examManager.ClearRandom(paper.Id, request.IsClear);

                    await SetRandomConfigs(request.ConfigList, paper);

                    await _examManager.PaperRandomSet(paper);
                    await _authManager.AddAdminLogAsync("重新发布试卷", $"{paper.Title}");
                    await _authManager.AddStatLogAsync(StatType.ExamUpdate, "重新发布试卷", paper.Id, paper.Title, oldPaper);

                    _examManager.ArrangeExamTask(paper.Id);
                }
                else
                {
                    await _authManager.AddAdminLogAsync("修改试卷", $"{paper.Title}");
                    await _authManager.AddStatLogAsync(StatType.ExamUpdate, "修改试卷", paper.Id, paper.Title, oldPaper);
                }

                await _examPaperRepository.UpdateAsync(paper);

                if (request.IsUpdateDateTime)
                {
                    await _examPaperUserRepository.UpdateExamDateTimeAsync(paper.Id, paper.ExamBeginDateTime.Value, paper.ExamEndDateTime.Value);
                }
                if (request.IsUpdateExamTimes)
                {
                    await _examPaperUserRepository.UpdateExamTimesAsync(paper.Id, paper.ExamTimes);
                }
                if (oldPaper.Title != paper.Title)
                {
                    await _examPaperUserRepository.UpdateKeyWordsAsync(paper.Id, paper.Title);
                    await _examPaperStartRepository.UpdateKeyWordsAsync(paper.Id, paper.Title);
                }
                if (oldPaper.Moni != paper.Moni)
                {
                    await _examPaperUserRepository.UpdateMoniAsync(paper.Id, paper.Moni);
                }
                if (oldPaper.LockedApp != paper.LockedApp)
                {
                    await _examPaperUserRepository.UpdateLockedAppAsync(paper.Id, paper.LockedApp);
                }
            }
            else
            {
                paper.SubmitType = request.SubmitType;
                paper.CompanyId = adminAuth.CurCompanyId;
                paper.CreatorId = admin.Id;
                paper.DepartmentId = admin.DepartmentId;
                paper.CompanyParentPath = adminAuth.CompanyParentPath;
                paper.DepartmentParentPath = admin.DepartmentParentPath;

                var paperId = await _examPaperRepository.InsertAsync(paper);


                paper = await _examPaperRepository.GetAsync(paperId);

                if (paper.SeparateStorage)
                {
                    await _examPaperRandomRepository.CreateSeparateStorageAsync(paper.Id);
                    await _examPaperRandomConfigRepository.CreateSeparateStorageAsync(paper.Id);
                    await _examPaperAnswerRepository.CreateSeparateStorageAsync(paper.Id);
                    await _examPaperRandomTmRepository.CreateSeparateStorageAsync(paper.Id);
                }

                await SetRandomConfigs(request.ConfigList, paper);


                if (request.SubmitType == SubmitType.Submit)
                {
                    await _examManager.PaperRandomSet(paper);
                    await _authManager.AddAdminLogAsync("发布试卷", $"{paper.Title}");
                    await _authManager.AddStatLogAsync(StatType.ExamAdd, "发布试卷", paper.Id, paper.Title);
                    await _authManager.AddStatCount(StatType.ExamAdd);

                    _examManager.ArrangeExamTask(paper.Id);
                }
                else
                {
                    await _authManager.AddAdminLogAsync("保存试卷", $"{paper.Title}");
                    await _authManager.AddStatLogAsync(StatType.ExamAdd, "保存试卷", paper.Id, paper.Title);
                    await _authManager.AddStatCount(StatType.ExamAdd);
                }

                if (tree != null)
                {
                    paper.TreeParentPath = tree.ParentPath;
                }
                await _examPaperRepository.UpdateAsync(paper);
            }

            return new BoolResult
            {
                Value = true
            };
        }

        private async Task SetRandomConfigs(List<ExamPaperRandomConfig> randomConfigs, ExamPaper paper)
        {
            await _examPaperRandomConfigRepository.DeleteByPaperAsync(paper.Id);

            if (paper.TmRandomType != ExamPaperTmRandomType.RandomNone)
            {
                var txIds = new List<int>();
                if (randomConfigs != null && randomConfigs.Count > 0)
                {
                    foreach (var randomConfig in randomConfigs)
                    {
                        if (randomConfig.Nandu1TmCount > 0 || randomConfig.Nandu2TmCount > 0 || randomConfig.Nandu3TmCount > 0 || randomConfig.Nandu4TmCount > 0 || randomConfig.Nandu5TmCount > 0)
                        {
                            randomConfig.ExamPaperId = paper.Id;
                            txIds.Add(randomConfig.TxId);
                            await _examPaperRandomConfigRepository.InsertAsync(randomConfig);
                        }

                    }
                }
                paper.TxIds = txIds;
            }
        }
    }


}
