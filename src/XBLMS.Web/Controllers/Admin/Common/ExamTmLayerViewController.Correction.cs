using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class ExamTmLayerViewController
    {
        [HttpGet, Route(RouteCorrection)]
        public async Task<ActionResult<GetCorrectionResult>> GetCorrection([FromQuery] IdRequest request)
        {
            var correction = await _examTmCorrectionRepository.GetAsync(request.Id);
            var itemNew = await _examTmRepository.GetAsync(correction.TmId);
            var txNew = await _examTxRepository.GetAsync(itemNew.TxId);
            itemNew.Set("OptionsStr", ListUtils.ToString(ListUtils.ToList(itemNew.Get("options"))));
            itemNew.Set("TxName", txNew?.Name);

            var item = TranslateUtils.JsonDeserialize<ExamTm>(correction.TmSourceObject);
            var tx = await _examTxRepository.GetAsync(item.TxId);
            item.Set("OptionsStr", ListUtils.ToString(ListUtils.ToList(item.Get("options"))));
            item.Set("TxName", tx?.Name);

            var user = await _userRepository.GetByUserIdAsync(correction.UserId);
            var info = new ExamTmCorrection();
            info.CreatedDate = correction.CreatedDate;
            info.UserId = correction.UserId;
            info.AuditStatus = correction.AuditStatus;
            info.Reason = correction.Reason;
            info.AuditResaon = correction.AuditResaon;
            info.AuditDate = correction.AuditDate;
            info.Set("AuditStatusStr", correction.AuditStatus.GetDisplayName());
            info.Set("UserDisplayName", user?.DisplayName);

            return new GetCorrectionResult
            {
                ItemNew = itemNew,
                Item = item,
                Info = info
            };
        }
    }
}
