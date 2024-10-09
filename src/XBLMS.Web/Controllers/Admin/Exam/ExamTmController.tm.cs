using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class ExamTmController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetSearchResults>> GetSearch([FromQuery] GetSearchRequest request)
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



            var group = await _examTmGroupRepository.GetAsync(request.TmGroupId);

            var (total, list) = await _examTmRepository.GetListAsync(group, treeIds, request.TxId, request.Nandu, request.Keyword, request.Order, request.OrderType, request.IsStop, request.PageIndex, request.PageSize);
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

        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var tm = await _examTmRepository.GetAsync(request.Id);
            if (tm == null) return this.NotFound();
            await _examTmRepository.DeleteAsync(request.Id);
            await _authManager.AddAdminLogAsync("删除题目", $"{tm.Title}");

            return new BoolResult
            {
                Value = true
            };
        }
        [HttpPost, Route(RouteDeleteSearch)]
        public async Task<ActionResult<BoolResult>> DeleteSearch([FromBody] GetDeletesRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            foreach (var id in request.Ids)
            {
                var info = await _examTmRepository.GetAsync(id);
                if (info == null) continue;
                await _examTmRepository.DeleteAsync(info.Id);
                await _authManager.AddAdminLogAsync("删除题目", $"{info.Title}");
            }
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
