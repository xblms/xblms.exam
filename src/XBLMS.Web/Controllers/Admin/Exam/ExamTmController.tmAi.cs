using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmController
    {
        [HttpGet, Route(RouteAi)]
        public async Task<ActionResult<GetAiTmResult>> GetAi([FromQuery] GetAiTmRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var admin = await _authManager.GetAdminAsync();
            var (total, list) = await _examTmAiRepository.GetListAsync(adminAuth, request.Status, request.KeyWords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var tm in list)
                {
                    await _examManager.GetTmInfoNoTree(tm);
                }
            }
            return new GetAiTmResult
            {
                Total = total,
                List = list
            };
        }
        [HttpGet, Route(RouteAiPublish)]
        public async Task<ActionResult<GetAiPublishResult>> GetAiPublish()
        {
            var config = await _configRepository.GetAsync();
            var txList = await _examTxRepository.GetListAsync();
            txList = txList.FindAll(tx => tx.ExamTxBase != Enums.ExamTxBase.Zuheti).ToList();
            return new GetAiPublishResult
            {
                TxList = txList,
                AiServe = config.AiServe
            };
        }

        [HttpPost, Route(RouteAiPublish)]
        public async Task<ActionResult<GetAiPublishTmResult>> AiPublish([FromBody] GetAiPublishTmRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var admin = await _authManager.GetAdminAsync();
            var tx = await _examTxRepository.GetAsync(request.TxId);
            var aiResult = await _aiTaskService.ExcutionTm(tx, request.Zsd);
            return new GetAiPublishTmResult
            {
                Success = aiResult.Success,
                Msg = aiResult.Msg,
                Item = aiResult.Tm
            };
        }

        [HttpPost, Route(RouteAiPublishSave)]
        public async Task<ActionResult<BoolResult>> AiPublishSave([FromBody] GetAiPublishTmSaveRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var admin = await _authManager.GetAdminAsync();

            if (request.TmList != null && request.TmList.Count > 0)
            {
                foreach (var item in request.TmList)
                {
                    var tm = new ExamTmAi();
                    tm.CompanyId = adminAuth.CurCompanyId;
                    tm.DepartmentId = admin.DepartmentId;
                    tm.CreatorId = admin.Id;
                    tm.CompanyParentPath = adminAuth.CompanyParentPath;
                    tm.DepartmentParentPath = admin.DepartmentParentPath;
                    tm.Title = item.Title;
                    tm.TxId = item.TxId;
                    tm.Zhishidian = item.Zhishidian;
                    tm.Nandu = item.Nandu;
                    tm.Answer = item.Answer;
                    tm.Score = item.Score;
                    tm.Jiexi = item.Jiexi;

                    tm.Set("options", tm.Get("options"));
                    tm.Set("optionsValues", tm.Get("optionsValues"));

                    tm.Id = await _examTmAiRepository.InsertAsync(tm);

                    await _authManager.AddAdminLogAsync("AI新增题目", $"{_examManager.GetTmTitle(tm)}");
                }
            }
            return new BoolResult
            {
                Value = true
            };
        }

        [HttpPost, Route(RouteAiDel)]
        public async Task<ActionResult<BoolResult>> AiDelete([FromBody] IdRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var info = await _examTmAiRepository.GetAsync(request.Id);
            await _examTmAiRepository.DeleteAsync(request.Id);
            await _authManager.AddAdminLogAsync("AI删除题目", $"{StringUtils.StripTags(info.Title)}");
            return new BoolResult
            {
                Value = true
            };
        }
        [HttpPost, Route(RouteAiDels)]
        public async Task<ActionResult<BoolResult>> AiDeletes()
        {
            var adminAuth = await _authManager.GetAdminAuth();
            await _examTmAiRepository.DeleteAllAsync(adminAuth);
            await _authManager.AddAdminLogAsync("AI删除题目", $"批量删除已入库的题目");
            return new BoolResult
            {
                Value = true
            };
        }
        [HttpPost, Route(RouteAiRuku)]
        public async Task<ActionResult<BoolResult>> AiRuku([FromBody] GetAiRukuRequest request)
        {
            var info = await _examTmAiRepository.GetAsync(request.Id);
            await _examTmAiRepository.StockedAsync(request.Id);
            info.TreeId = request.TreeId;
            var tree = await _examTmTreeRepository.GetAsync(info.TreeId);
            if (tree != null)
            {
                info.TreeParentPath = tree.ParentPath;
            }
            await _examTmRepository.InsertAsync(info);
            await _authManager.AddAdminLogAsync("AI题目入库", $"{StringUtils.StripTags(info.Title)}");
            return new BoolResult
            {
                Value = true
            };
        }
        [HttpPost, Route(RouteAiRukus)]
        public async Task<ActionResult<BoolResult>> AiRukus([FromBody] IdRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var tree = await _examTmTreeRepository.GetAsync(request.Id);
            var (total, list) = await _examTmAiRepository.GetStockedAllAsync(adminAuth);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    await _examTmAiRepository.StockedAsync(item.Id);
                    item.TreeId = request.Id;
                    item.TreeParentPath = tree.ParentPath;
                    await _examTmRepository.InsertAsync(item);
                }
                await _authManager.AddAdminLogAsync("AI题目入库", $"批量入库题目");
            }
            return new BoolResult
            {
                Value = true
            };
        }

        public class GetAiTmRequest
        {
            public string KeyWords { get; set; }
            public int Status { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetAiTmResult
        {
            public int Total { get; set; }
            public List<ExamTmAi> List { get; set; }
        }
        public class GetAiPublishResult
        {
            public bool AiServe { get; set; }
            public List<ExamTx> TxList { get; set; }
        }
        public class GetAiPublishTmRequest
        {
            public int TxId { get; set; }
            public string Zsd { get; set; }
        }
        public class GetAiPublishTmSaveRequest
        {
            public List<ExamTmAi> TmList { get; set; }
        }
        public class GetAiPublishTmResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public ExamTmAi Item { get; set; }
        }
        public class GetAiRukuRequest
        {
            public int Id { get; set; }
            public int TreeId { get; set; }
        }
    }
}
