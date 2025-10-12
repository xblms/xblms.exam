using Datory;
using Datory.Annotations;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("study_CourseWareUser")]
    public class StudyCourseWareUser : Entity
    {
        [DataColumn]
        public int PlanId { get; set; }
        [DataColumn]
        public int CourseId { get; set; }
        [DataColumn]
        public int CourseWareId { get; set; }
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public StudyStatType State { get; set; }
        [DataColumn]
        public int TotalDuration { get; set; }
        [DataColumn]
        public int CurrentDuration { get; set; }
        /// <summary>
        /// 上次学习
        /// </summary>
        [DataColumn]
        public bool StudyCurrent { get; set; }
    }
}
