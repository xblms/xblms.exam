using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Services
{
    public interface IAiTaskService
    {
        Task<(bool success, DoAI.DoAI_Version_Result result, string msg)> ExcutionStatus(string host);
        Task<(bool success, DoAI.DoAI_RunningModels_Result result, string msg)> ExcutionRunningModels(string host);
        Task<DoAI.DoAI_Tm_Result> ExcutionTm(ExamTx tx, string zsd);
    }
}
