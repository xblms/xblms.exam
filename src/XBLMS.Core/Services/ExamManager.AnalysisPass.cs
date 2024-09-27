using System;
using System.Drawing.Printing;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task<(double allPassPercent, int allTotal, double moniPassPercent, int moniTotal, double paperPassPercent, int paperTotal)> AnalysisMorePass(int userId)
        {

            var passTotal = 0;
            var paperTotal = 0;

            var moniPassTotal = 0;
            var paperPassTotal = 0;

            var moniPaperTotal = 0;
            var paperPaperTotal = 0;

            double allPass = 0;
            double moniPass = 0;
            double paperPass = 0;


            var (total, list) = await _examPaperStartRepository.GetListAsync(userId, "", "", "", 1, int.MaxValue);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    paperTotal++;

                    var paper = await _examPaperRepository.GetAsync(item.ExamPaperId);
                    if (paper != null)
                    {
                        if (item.Score > paper.PassScore)
                        {
                            passTotal++;
                            if (paper.Moni)
                            {
                                moniPassTotal++;
                            }
                            else
                            {
                                paperPassTotal++;
                            }
                        }
                        if (paper.Moni)
                        {
                            moniPaperTotal++;
                        }
                        else
                        {
                            paperPaperTotal++;
                        }
                    }
                }
            }


            if (passTotal > 0 && paperTotal > 0)
            {
                allPass = Math.Round((Convert.ToDouble(passTotal) / paperTotal * 100), 0);
            }

            if (moniPassTotal > 0 && moniPaperTotal > 0)
            {
                moniPass = Math.Round((Convert.ToDouble(moniPassTotal) / moniPaperTotal * 100), 0);
            }

            if (paperPassTotal > 0 && paperPaperTotal > 0)
            {
                paperPass = Math.Round((Convert.ToDouble(paperPassTotal) / paperPaperTotal * 100), 0);
            }


            return (allPass, paperTotal, moniPass, moniPaperTotal, paperPass, paperPaperTotal);
        }
    }
}
