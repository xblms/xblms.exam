using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Core.Utils.Office;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmSelectController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetSearchResults>> Get([FromQuery] GetSearchRequest request)
        {
            var treeIds = new List<int>();
            if (request.TreeId > 0)
            {
                if (request.TreeIsChildren)
                {
                    treeIds = await _examTmTreeRepository.GetIdsAsync(request.TreeId);
                }
                else
                {
                    treeIds.Add(request.TreeId);
                }
            }
            var group = await _examTmGroupRepository.GetAsync(request.Id);

            var (total, list) = await _examTmRepository.GetListAsync(group.TmIds, treeIds, request.TxId, request.Nandu, request.Keyword, request.Order, request.OrderType, request.IsStop, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var tm in list)
                {
                    await _examManager.GetTmInfo(tm);
                }
            }

            return new GetSearchResults
            {
                Items = list,
                Total = total,
            };
        }
        [RequestSizeLimit(long.MaxValue)]
        [HttpGet, Route(RouteGetIn)]
        public async Task<ActionResult<GetSearchResults>> GetSelect([FromQuery] IdRequest request)
        {
            var group = await _examTmGroupRepository.GetAsync(request.Id);

            var list = await _examTmRepository.GetListAsync(group.TmIds);
            if (list != null && list.Count > 0)
            {
                foreach (var tm in list)
                {
                    await _examManager.GetTmInfo(tm);
                }
                list = list.ToList().OrderBy(tm => tm.Get("TxTaxis")).ToList();
            }

            return new GetSearchResults
            {
                Items = list
            };
        }
    }
}
