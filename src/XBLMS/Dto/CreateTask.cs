using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Dto
{
    public class CreateTask
    {
        public CreateTask(CreateType createType,ExamPaperAnswer examPaperAnswer)
        {
            ExamPaperAnswer = examPaperAnswer;
            CreateType = createType;
        }
        public CreateTask(CreateType createType, ExamPaperAnswerSmall examPaperAnswerSmall)
        {
            ExamPaperAnswerSmall = examPaperAnswerSmall;
            CreateType = createType;
        }
        public CreateTask(CreateType createType,int startId)
        {
            StartId = startId;
            CreateType = createType;
        }
        public CreateType CreateType { get; set; }
        public ExamPaperAnswer ExamPaperAnswer { get; set; }
        public ExamPaperAnswerSmall ExamPaperAnswerSmall { get; set; }
        public int StartId { get; set; }
    }
}
