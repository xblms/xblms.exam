using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class ExamPaperUserLayerViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetList([FromQuery] IdRequest request)
        {
            var start = await _examPaperStartRepository.GetAsync(request.Id);

            var user = await _userRepository.GetByUserIdAsync(start.UserId);

            var paper = await _examPaperRepository.GetAsync(start.ExamPaperId);

            if (paper == null || start == null) { return NotFound(); }

            var randomId = start.ExamPaperRandomId;
            var startId = request.Id;

            if (randomId == 0) { return this.Error("试卷发布有问题！"); }

            var configs = await _examPaperRandomConfigRepository.GetListAsync(paper.Id);
            if (configs == null || configs.Count == 0) { return this.Error("试卷发布有问题：找不到任何题目！"); }

            var paperTmTotal = 0;

            var tmIndex = 1;
            foreach (var config in configs)
            {
                var tms = await _examPaperRandomTmRepository.GetListAsync(randomId, config.TxId);
                if (tms != null && tms.Count > 0)
                {
                    paperTmTotal += tms.Count;

                    foreach (var item in tms)
                    {
                        await _examManager.GetTmInfoByPaperViewAdmin(item, paper, startId);
                        item.Set("TmIndex", tmIndex);
                        tmIndex++;
                    }
                    config.Set("TmList", tms);
                }
            }

            paper.Set("TmTotal", paperTmTotal);
            paper.Set("StartId", startId);

            paper.Set("UserDisplayName", user.DisplayName);
            paper.Set("UserAvatar", user.AvatarUrl);

            paper.Set("UseTime", DateUtils.SecondToHms(start.ExamTimeSeconds));
            paper.Set("UserScore", start.Score);
            paper.Set("UserOScore", start.ObjectiveScore);
            paper.Set("UserSScore", start.SubjectiveScore);

            return new GetResult
            {
                Watermark = $"{user.DisplayName}-{user.UserName}-{paper.Title}-{start.EndDateTime}",
                Item = paper,
                TxList = configs
            };
        }
    }
}
