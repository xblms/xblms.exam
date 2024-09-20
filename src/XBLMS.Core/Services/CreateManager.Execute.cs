using System;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class CreateManager
    {
        public async Task ExecuteSubmitAnswerAsync(ExamPaperAnswer examPaperAnswer)
        {
            var tm = await _databaseManager.ExamPaperRandomTmRepository.GetAsync(examPaperAnswer.RandomTmId);
            if(tm.Answer== examPaperAnswer.Answer)
            {
                examPaperAnswer.Score = tm.Score;
            }
            await _databaseManager.ExamPaperAnswerRepository.UpdateAsync(examPaperAnswer);
        }
        public async Task ExecuteSubmitPaperAsync(int startId)
        {
            //Thread.Sleep(1000 * 10);
            var start = await _databaseManager.ExamPaperStartRepository.GetAsync(startId);
            var paper = await _databaseManager.ExamPaperRepository.GetAsync(start.ExamPaperId);

            bool hasZhuguanti = false;
            var txList = await _databaseManager.ExamTxRepository.GetListAsync();
            if(txList!=null && txList.Count > 0)
            {
                txList = txList.Where(tx => paper.TxIds.Contains(tx.Id)).ToList();
                if (txList!=null && txList.Count > 0)
                {
                    foreach (var item in txList)
                    {
                        if(item.ExamTxBase==ExamTxBase.Tiankongti || item.ExamTxBase == ExamTxBase.Jiandati)
                        {
                            hasZhuguanti = true;
                            break;
                        }
                    }
                }
            }
            if (hasZhuguanti)
            {
                if (paper.IsAutoScore)
                {
                    start.IsMark = true;
                }
                else
                {
                    start.IsMark = false;
                }
            }
            else
            {
                start.IsMark = true;
            }

            start.IsSubmit = true;
            start.EndDateTime = DateTime.Now;

            var sumScore = await _databaseManager.ExamPaperAnswerRepository.ScoreSumAsync(startId);
            start.Score = sumScore;
            await _databaseManager.ExamPaperStartRepository.UpdateAsync(start);

            if(start.IsMark && start.IsSubmit && start.Score >= paper.PassScore)
            {
                await AwardCer(paper, startId, start.UserId);
            }
        }
    }
}
