using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperEditController
    {
        [HttpPost, Route(RouteGetConfig)]
        public async Task<ActionResult<GetConfigResult>> GetConfig([FromBody] GetConfigRequest request)
        {
            var result = new List<ExamPaperRandomConfig>();
            var txList = await _examTxRepository.GetListAsync();
            if (request.TxIds != null && request.TxIds.Count > 0)
            {
                txList = txList.Where(tx => request.TxIds.Contains(tx.Id)).ToList();
            }

            var tmIds = new List<int>();
            var tmGroupIds = request.TmGroupIds;

            var allTm = false;
            if (tmGroupIds != null && tmGroupIds.Count > 0)
            {
                foreach (var tmGroupId in tmGroupIds)
                {
                    var tmGroup = await _examTmGroupRepository.GetAsync(tmGroupId);
                    if (tmGroup != null)
                    {
                        if (tmGroup.GroupType == TmGroupType.Fixed && tmGroup.TmIds != null && tmGroup.TmIds.Count > 0)
                        {
                            tmIds.AddRange(tmGroup.TmIds);
                        }
                        if (tmGroup.GroupType == TmGroupType.Range)
                        {
                            var tmIdsByGroup = await _examTmRepository.Group_RangeIdsAsync(tmGroup);
                            if (tmIdsByGroup != null && tmIdsByGroup.Count > 0)
                            {
                                tmIds.AddRange(tmIdsByGroup);
                            }
                        }
                        if (tmGroup.GroupType == TmGroupType.All)
                        {
                            allTm = true;
                        }
                    }
                }

            }

            var tmTotal = 0;
            foreach (var tx in txList)
            {
             
                if (allTm)
                {
                    tmIds = null;
                }

                var count1 = await _examTmRepository.GetCountAsync(tmIds, tx.Id, 1);
                var count2 = await _examTmRepository.GetCountAsync(tmIds, tx.Id, 2);
                var count3 = await _examTmRepository.GetCountAsync(tmIds, tx.Id, 3);
                var count4 = await _examTmRepository.GetCountAsync(tmIds, tx.Id, 4);
                var count5 = await _examTmRepository.GetCountAsync(tmIds, tx.Id, 5);



                tmTotal += count1 + count2 + count3 + count4 + count5;
                result.Add(new ExamPaperRandomConfig
                {
                    TxId = tx.Id,
                    TxName = tx.Name,
                    TxScore = tx.Score,
                    TxTaxis = tx.Taxis,
                    Nandu1TmCount = 0,
                    Nandu2TmCount = 0,
                    Nandu3TmCount = 0,
                    Nandu4TmCount = 0,
                    Nandu5TmCount = 0,
                    Nandu1TmTotal = count1,
                    Nandu2TmTotal = count2,
                    Nandu3TmTotal = count3,
                    Nandu4TmTotal = count4,
                    Nandu5TmTotal = count5
                });
            }
            AutoConfigSet(tmTotal, 0, result);
            return new GetConfigResult
            {
                Items = result
            };
        }

        private void AutoConfigSet(int tmTotal, int setTotal, List<ExamPaperRandomConfig> configs)
        {
            if (tmTotal > 0)
            {
                if (tmTotal > 100)
                {
                    if (setTotal != 100)
                    {
                        foreach (var config in configs)
                        {
                            if (config.Nandu1TmCount < config.Nandu1TmTotal && setTotal != 100)
                            {
                                config.Nandu1TmCount++;
                                setTotal++;
                            }
                            if (config.Nandu2TmCount < config.Nandu2TmTotal && setTotal != 100)
                            {
                                config.Nandu2TmCount++;
                                setTotal++;
                            }
                            if (config.Nandu3TmCount < config.Nandu3TmTotal && setTotal != 100)
                            {
                                config.Nandu3TmCount++;
                                setTotal++;
                            }
                            if (config.Nandu4TmCount < config.Nandu4TmTotal && setTotal != 100)
                            {
                                config.Nandu4TmCount++;
                                setTotal++;
                            }
                            if (config.Nandu5TmCount < config.Nandu5TmTotal && setTotal != 100)
                            {
                                config.Nandu5TmCount++;
                                setTotal++;
                            }
                        }

                        AutoConfigSet(tmTotal, setTotal, configs);
                    }
                }
                else
                {
                    foreach (var config in configs)
                    {
                        config.Nandu1TmCount = config.Nandu1TmTotal;
                        config.Nandu2TmCount = config.Nandu2TmTotal;
                        config.Nandu3TmCount = config.Nandu3TmTotal;
                        config.Nandu4TmCount = config.Nandu4TmTotal;
                        config.Nandu5TmCount = config.Nandu5TmTotal;
                    }
                }
            }

        }
    }
}
