using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmController
    {
        [HttpGet, Route(RouteAiEdit)]
        public async Task<ActionResult<GetAiTmEditResult>> GetAiEdit([FromQuery] IdRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var resultSmalls = new List<ExamTmSmall>();
            var tm = new ExamTmAi();
            if (request.Id > 0)
            {
                tm = await _examManager.GetAiTmInfo(request.Id);
            }
            var txList = await _examTxRepository.GetListAsync();
            txList = txList.FindAll(tx => tx.ExamTxBase != ExamTxBase.Zuheti).ToList();

            return new GetAiTmEditResult
            {
                Item = tm,
                TxList = txList
            };
        }

        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteAiEditSubmit)]
        public async Task<ActionResult<BoolResult>> SubmitAiTm([FromBody] GetAiTmEditRequest request)
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

            var txInfo = await _examTxRepository.GetAsync(info.TxId);
            if (txInfo.ExamTxBase == ExamTxBase.Duoxuanti)
            {
                info.Answer = info.Answer.Replace(",", "").Trim();
            }

            if (info.Id > 0)
            {
                var last = await _examTmAiRepository.GetAsync(info.Id);

                await _examTmAiRepository.UpdateAsync(info);
                await _authManager.AddAdminLogAsync("修改AI题目", $"{_examManager.GetTmTitle(info)}");
                await _authManager.AddStatLogAsync(StatType.ExamTmAIUpdate, "修改AI题目", last.Id, _examManager.GetTmTitle(info), last);
            }

            return new BoolResult
            {
                Value = true
            };
        }

        public class GetAiTmEditResult
        {
            public ExamTmAi Item { get; set; }
            public List<ExamTx> TxList { get; set; }
        }

        public class GetAiTmEditRequest
        {
            public ExamTmAi Item { get; set; }
        }
    }
}
