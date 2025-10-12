using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteReadySubmit)]
        public async Task<ActionResult<IntResult>> SubmitReady([FromBody] GetSubmitReadyRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            request.Item.UserId = user.Id;
            request.Item.IsCreate = true;

            var createId = await _examPracticeRepository.InsertAsync(request.Item);
            var practiceId = 0;
            if (createId > 0)
            {
                var practice = request.Item;
                practice.IsCreate = false;
                practice.ParentId = createId;
                practice.TmIds = ListUtils.GetRandomList(request.Item.TmIds, practice.MineTmCount);
                practiceId = await _examPracticeRepository.InsertAsync(request.Item);
            }
            return new IntResult
            {
                Value = practiceId
            };

        }


        [HttpGet, Route(RouteReady)]
        public async Task<ActionResult<GetReadyRequest>> ReadyGet()
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();


            var txList = await _examTxRepository.GetListAsync();
            var txIds = new List<int>();
            foreach (var tx in txList)
            {
                txIds.Add(tx.Id);
            }

            List<int> nds = [1, 2, 3, 4, 5];

            var (tmCount, tmIds, tmGroupIds) = await _examManager.PracticeGetTmids(user, txIds, nds, null);

            return new GetReadyRequest
            {
                TmGroupIds = tmGroupIds,
                TxList = txList,
                Item = new ExamPractice()
                {
                    Title = DateTime.Now.ToString(DateUtils.FormatStringDateTimeJoinCN),
                    TxIds = txIds,
                    Zsds = [],
                    Nds = nds,
                    TmCount = tmCount,
                    TmIds = tmIds
                }
            };
        }

        [HttpPost, Route(RouteReadySearch)]
        public async Task<ActionResult<GetReadySearchResult>> ReadyGet([FromBody] GetReadySearchRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var (tmCount, tmIds) = await _examManager.PracticeGetTmids(request.TmGroupIds, request.TxIds, request.Nds, request.Zsds);
            return new GetReadySearchResult
            {
                TmCount = tmCount,
                TmIds = tmIds
            };
        }
    }
}
