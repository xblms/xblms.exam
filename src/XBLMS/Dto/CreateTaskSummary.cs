using System.Collections.Generic;

namespace XBLMS.Dto
{
    public class CreateTaskSummary
    {
        public CreateTaskSummary(List<CreateTaskSummaryItem> tasks, int answerCount, int paperCount)
        {
            Tasks = tasks;
            AnswerCount = answerCount;
            PaperCount = paperCount;
        }

        public List<CreateTaskSummaryItem> Tasks { get; }

        public int AnswerCount { get; }

        public int PaperCount { get; }

    }
}
