using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IExamManager
    {
        Task<(int total, List<int>)> PracticeGetTmids(int userId, List<int> txIds, List<int> nds, List<string> zsds);
    }

}
