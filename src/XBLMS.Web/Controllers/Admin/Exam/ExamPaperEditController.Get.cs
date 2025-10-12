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
    public partial class ExamPaperEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var paper = new ExamPaper();
            paper.Title = "在线考试-" + StringUtils.PadZeroes(await _examPaperRepository.MaxAsync() + 1, 5);
            if (request.Id > 0)
            {
                paper = await _examPaperRepository.GetAsync(request.Id);
            }

            var tree = await _examManager.GetExamPaperTreeCascadesAsync(adminAuth);
            var txs = await _examTxRepository.GetListAsync();

            var cers = await _examCerRepository.GetListAsync(adminAuth);
            var tmGroups = await _examTmGroupRepository.GetListAsync(adminAuth, string.Empty, true);
            var userGroups = await _userGroupRepository.GetListAsync(adminAuth, true);
            var configList = await _examPaperRandomConfigRepository.GetListAsync(request.Id);
            var fixedGroups = new List<ExamTmGroup>();
            if (tmGroups != null && tmGroups.Count > 0)
            {
                fixedGroups.AddRange(tmGroups.Where(group => group.GroupType == TmGroupType.Fixed && group.TmTotal > 0).ToList());
            }

            var config = await _configRepository.GetAsync();
            return new GetResult
            {
                Item = paper,
                PaperTree = tree,
                TxList = txs,
                CerList = cers,
                TmGroupList = tmGroups,
                TmFixedGroupList = fixedGroups,
                UserGroupList = userGroups,
                ConfigList = configList,
                SystemCode = config.SystemCode
            };

        }

    }
}
