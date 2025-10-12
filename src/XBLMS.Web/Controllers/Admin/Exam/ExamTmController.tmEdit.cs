using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmController
    {

        [HttpGet, Route(RouteEdit)]
        public async Task<ActionResult<GetEditResult>> Get([FromQuery] IdRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var resultSmalls = new List<ExamTmSmall>();
            var tm = new ExamTm();
            if (request.Id > 0)
            {
                tm = await _examManager.GetTmInfo(request.Id);

                var smallList = await _examTmSmallRepository.GetListAsync(tm.Id);

                if (smallList != null && smallList.Count > 0)
                {
                    for (var i = 0; i < smallList.Count; i++)
                    {
                        var smallTm = smallList[i];
                        resultSmalls.Add(await _examManager.GetSmallTmInfo(smallTm.Id));
                    }
                }
            }
            var txList = await _examTxRepository.GetListAsync();
            var tmTree = await _examManager.GetExamTmTreeCascadesAsync(adminAuth);

            return new GetEditResult
            {
                Item = tm,
                TxList = txList,
                TmTree = tmTree,
                SmallList = resultSmalls
            };
        }

        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteEditSubmit)]
        public async Task<ActionResult<BoolResult>> SubmitTm([FromBody] GetEditRequest request)
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

            var info = request.Item;

            var tree = await _examTmTreeRepository.GetAsync(info.TreeId);
            if (tree != null)
            {
                info.TreeParentPath = tree.ParentPath;
            }

            var txInfo = await _examTxRepository.GetAsync(info.TxId);
            if (txInfo.ExamTxBase == ExamTxBase.Duoxuanti)
            {
                info.Answer = info.Answer.Replace(",", "").Trim();
            }

            if (info.Id > 0)
            {
                var last = await _examTmRepository.GetAsync(info.Id);
               
                await _examTmRepository.UpdateAsync(info);
                await _authManager.AddAdminLogAsync("修改题目", $"{StringUtils.StripTags(info.Title)}");
                await _authManager.AddStatLogAsync(StatType.ExamTmUpdate, "修改题目", last.Id, StringUtils.StripTags(info.Title), last);
            }
            else
            {
                if (await _examTmRepository.ExistsAsync(info.Title, info.TxId))
                {
                    return this.Error("已存在相同的题目");
                }

                info.CompanyId = adminAuth.CurCompanyId;
                info.DepartmentId = admin.DepartmentId;
                info.CreatorId = admin.Id;
                info.CompanyParentPath = adminAuth.CompanyParentPath;
                info.DepartmentParentPath = admin.DepartmentParentPath;

                info.Id = await _examTmRepository.InsertAsync(info);

                await _authManager.AddAdminLogAsync("新增题目", $"{StringUtils.StripTags(info.Title)}");
                await _authManager.AddStatLogAsync(StatType.ExamTmAdd, "新增题目", info.Id, StringUtils.StripTags(info.Title));
                await _authManager.AddStatCount(StatType.ExamTmAdd);
            }

            if (txInfo.ExamTxBase == ExamTxBase.Zuheti)
            {
                if (request.Smalls != null && request.Smalls.Count > 0)
                {
                    for (var i = 0; i < request.Smalls.Count; i++)
                    {
                        var smallTm = request.Smalls[i];
                        var smallTxInfo = await _examTxRepository.GetAsync(smallTm.TxId);
                        if (smallTxInfo.ExamTxBase == ExamTxBase.Duoxuanti)
                        {
                            smallTm.Answer = smallTm.Answer.Replace(",", "").Trim();

                        }
                        smallTm.ParentId = info.Id;
                        if (smallTm.Id > 0)
                        {
                            await _examTmSmallRepository.UpdateAsync(smallTm);
                        }
                        else
                        {
                            await _examTmSmallRepository.InsertAsync(smallTm);
                        }
                    }

                }
            }
            else
            {
                if (info.Id > 0)
                {
                    await _examTmSmallRepository.DeleteAsync(info.Id);
                }
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
