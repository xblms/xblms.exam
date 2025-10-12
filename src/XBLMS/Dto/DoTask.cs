using XBLMS.Enums;

namespace XBLMS.Dto
{
    public class DoTask
    {
        public int TaskId { get; set; }
        public TaskType TaskType { get; set; }
        public string TaskTypeName { get; set; }
        public string TaskTitle { get; set; }
        public string TaskBeginDateTime { get; set; }
        public string TaskEndDateTime { get; set; }
    }
}
