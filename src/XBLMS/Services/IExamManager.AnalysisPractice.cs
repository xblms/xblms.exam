using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IExamManager
    {
        /// <summary>
        /// 累计答题
        /// 累计答题正确率
        /// 共多少题
        /// 快速答题正确率
        /// 收藏共多少题
        /// 收藏答题正确率
        /// 错误共多少题
        /// 错误答题正确率
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<(int answerTmTotal, double answerPercent, int allTmTotal, double allAnswerPercent, int collectTmTotal, double collectAnswerPercent, int wrongTmTotal, double wrongAnswerPercent)> AnalysisPractice(int userId);
    }

}
