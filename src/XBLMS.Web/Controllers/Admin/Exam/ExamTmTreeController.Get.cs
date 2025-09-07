﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmTreeController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var trees = await _examManager.GetExamTmTreeCascadesAsync(true);
            var tmGroups = await _examTmGroupRepository.GetListWithoutLockedAsync();
            var txList = await _examTxRepository.GetListAsync();
            var orderTypeList = ListUtils.GetSelects<OrderType>();

            if (txList == null || txList.Count == 0)
            {
                await _examTxRepository.ResetAsync();
            }

            return new GetResult
            {
                Items = trees,
                TxList= txList,
                OrderTypeList = orderTypeList,
                TmGroups = tmGroups
            };
        }
    }
}
