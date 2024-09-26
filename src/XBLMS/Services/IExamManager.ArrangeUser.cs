using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IExamManager
    {
        Task Arrange(ExamPaper paper);
        Task Arrange(int paperId, int userId);
    }

}
