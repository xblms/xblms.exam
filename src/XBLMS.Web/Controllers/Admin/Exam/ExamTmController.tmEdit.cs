using Microsoft.AspNetCore.Mvc;
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
            var tm = new ExamTm();
            if (request.Id > 0)
            {
                tm = await _examManager.GetTmInfo(request.Id);
            }
            var txList = await _examTxRepository.GetListAsync();
            var tmTree = await _examManager.GetExamTmTreeCascadesAsync();

            return new GetEditResult
            {
                Item = tm,
                TxList = txList,
                TmTree = tmTree,
            };
        }

        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteEditSubmit)]
        public async Task<ActionResult<BoolResult>> SubmitTm([FromBody] ItemRequest<ExamTm> request)
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
            var info = request.Item;
            if (info.Id > 0)
            {
                var last = await _examTmRepository.GetAsync(info.Id);
                var txInfo = await _examTxRepository.GetAsync(info.TxId);
                if (txInfo.ExamTxBase == ExamTxBase.Duoxuanti)
                {
                    info.Answer = info.Answer.Replace(",", "").Trim();
                }
                await _examTmRepository.UpdateAsync(info);
                await _authManager.AddAdminLogAsync("修改题目", $"{StringUtils.StripTags(info.Title) }");
                await _authManager.AddStatLogAsync(StatType.ExamTmUpdate, "修改题目", last.Id, StringUtils.StripTags(info.Title), last);
            }
            else
            {
                if (await _examTmRepository.ExistsAsync(info.Title, info.TxId))
                {
                    return this.Error("已存在相同的题目");
                }
                var txInfo = await _examTxRepository.GetAsync(info.TxId);
                if (txInfo.ExamTxBase == ExamTxBase.Duoxuanti)
                {
                    info.Answer = info.Answer.Replace(",", "").Trim();
                }

                info.CompanyId = admin.CompanyId;
                info.DepartmentId = admin.DepartmentId;
                info.CreatorId = admin.Id;

                info.Id = await _examTmRepository.InsertAsync(info);

                await _authManager.AddAdminLogAsync("新增题目", $"{ StringUtils.StripTags(info.Title) }");
                await _authManager.AddStatLogAsync(StatType.ExamTmAdd, "新增题目", info.Id, StringUtils.StripTags(info.Title));
                await _authManager.AddStatCount(StatType.ExamTmAdd);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
