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
        public async Task ClearRandom(int examPaperId, bool isClear)
        {
            if (isClear)
            {
                await _examPaperRandomRepository.DeleteByPaperAsync(examPaperId);
                await _examPaperRandomTmRepository.DeleteByPaperAsync(examPaperId);
                await _examPaperRandomConfigRepository.DeleteByPaperAsync(examPaperId);
                await _examPaperUserRepository.ClearByPaperAsync(examPaperId);
                await _examPaperStartRepository.ClearByPaperAsync(examPaperId);
                await _examPaperAnswerRepository.ClearByPaperAsync(examPaperId);
            
            }
            else
            {
                var randomList = await _examPaperRandomRepository.GetListByPaperAsync(examPaperId);
                if (randomList != null && randomList.Count > 0)
                {
                    foreach (var random in randomList)
                    {
                        random.Locked = true;
                        await _examPaperRandomRepository.UpdateAsync(random);
                    }
                }

            }
        }
    }
}
