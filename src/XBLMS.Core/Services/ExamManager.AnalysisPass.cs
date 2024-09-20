using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Datory;
using Datory.Annotations;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Newtonsoft.Json.Converters;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task<(double allPassPercent, int allTotal, double moniPassPercent, int moniTotal, double paperPassPercent, int paperTotal)> AnalysisMorePass(int userId)
        {
            var paperIds = await _examPaperStartRepository.GetPaperIdsAsync(userId);
            var passTotal = 0;
            var paperTotal = 0;

            var moniPassTotal = 0;
            var paperPassTotal = 0;

            var moniPaperTotal = 0;
            var paperPaperTotal = 0;

            double allPass = 0;
            double moniPass = 0;
            double paperPass = 0;

            if (paperIds != null)
            {
                foreach (var paperId in paperIds)
                {
                    paperTotal++;
                    var paper = await _examPaperRepository.GetAsync(paperId);
                    var maxScore = await _examPaperStartRepository.GetMaxScoreAsync(userId, paperId);
                    if (paper != null)
                    {
                        if (maxScore.HasValue && maxScore.Value > paper.PassScore)
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
        public async Task<double> AnalysisPass(int userId)
        {
            var paperIds = await _examPaperStartRepository.GetPaperIdsAsync(userId);
            var passTotal = 0;
            var paperTotal = 0;
            if (paperIds != null)
            {
                foreach (var paperId in paperIds)
                {
                    paperTotal++;
                    var paper = await _examPaperRepository.GetAsync(paperId);
                    var maxScore = await _examPaperStartRepository.GetMaxScoreAsync(userId, paperId);
                    if (paper != null && maxScore.HasValue && maxScore.Value > paper.PassScore)
                    {
                        passTotal++;
                    }
                }
            }

            if (passTotal > 0 && paperTotal > 0)
            {
                return (Convert.ToDouble(passTotal) / paperTotal * 100);
            }
            return 0;
        }
    }
}
