using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IExamManager
    {
        Task<(int total, List<int>)> PracticeGetTmids(List<int> tmGroupIds, List<int> txIds, List<int> nds, List<string> zsds);
        Task<(int total, List<int>, List<int>)> PracticeGetTmids(User user, List<int> txIds, List<int> nds, List<string> zsds);
    }

}
