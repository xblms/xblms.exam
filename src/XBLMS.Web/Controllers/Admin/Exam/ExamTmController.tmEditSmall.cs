using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmController
    {
        [HttpGet, Route(RouteEditSmall)]
        public async Task<ActionResult<GetEditResult>> GetSmall([FromQuery] IdRequest request)
        {
            var tm = new ExamTm();
            if (request.Id > 0)
            {
                tm = await _examManager.GetSmallTmInfo(request.Id);
            }
            var txList = await _examTxRepository.GetListAsync();
            if (txList != null && txList.Count > 0)
            {
                txList = txList.Where(tx => tx.ExamTxBase != Enums.ExamTxBase.Zuheti).ToList();
            }

            return new GetEditResult
            {
                Item = tm,
                TxList = txList
            };
        }

        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteEditSmallSubmit)]
        public async Task<ActionResult<BoolResult>> SubmitSmallTm([FromBody] GetEditSmallRequest request)
        {

            var admin = await _authManager.GetAdminAsync();
            var info = request.Item;
            var txInfo = await _examTxRepository.GetAsync(info.TxId);
            if (txInfo.ExamTxBase == ExamTxBase.Duoxuanti)
            {
                info.Answer = info.Answer.Replace(",", "").Trim();
            }

            if (info.Id > 0)
            {
                await _examTmSmallRepository.UpdateAsync(info);
            }
            return new BoolResult
            {
                Value = true
            };
        }

        [HttpPost, Route(RouteEditSmallDelete)]
        public async Task<ActionResult<BoolResult>> DeleteSmall([FromBody] IdRequest request)
        {
            await _examTmSmallRepository.DeleteAsync(request.Id);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
