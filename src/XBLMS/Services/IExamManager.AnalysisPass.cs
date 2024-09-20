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
        /// 全部考试及格率
        /// 全部考试次数
        /// 模拟考试及格率
        /// 模拟考试次数
        /// 正式考试及格率
        /// 正式考试次数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<(double allPassPercent, int allTotal, double moniPassPercent, int moniTotal, double paperPassPercent, int paperTotal)> AnalysisMorePass(int userId);
        Task<double> AnalysisPass(int userId);
    }

}
