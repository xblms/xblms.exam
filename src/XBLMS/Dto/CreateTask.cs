using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Dto
{
    public class CreateTask
    {
        public CreateTask(CreateType createType, ExamPaperAnswer examPaperAnswer)
        {
            ExamPaperAnswer = examPaperAnswer;
            CreateType = createType;
        }
        public CreateTask(CreateType createType, int taskId)
        {
            TaskId = taskId;
            CreateType = createType;
        }
        public CreateTask(CreateType createType, ExamPaperAnswerSmall examPaperAnswerSmall)
        {
            ExamPaperAnswerSmall = examPaperAnswerSmall;
            CreateType = createType;
        }
        public CreateType CreateType { get; set; }
        public ExamPaperAnswer ExamPaperAnswer { get; set; }
        public ExamPaperAnswerSmall ExamPaperAnswerSmall { get; set; }
        public int TaskId { get; set; }
        public bool Wait { get; set; }
    }
}
