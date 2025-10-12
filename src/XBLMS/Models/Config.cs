using Datory;
using Datory.Annotations;
using System;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_Config")]
    public class Config : Entity
    {
        [DataColumn]
        public string DatabaseVersion { get; set; }

        public bool ExamTmCache { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Initialized => Id > 0;
        public bool IsLogAdmin { get; set; } = true;
        public bool IsLogUser { get; set; } = true;
        public bool IsLogError { get; set; } = true;
        public bool IsTimeThreshold { get; set; }
        public int TimeThreshold { get; set; } = 60;
        public string AdminDefaultPassword { get; set; } = "password@1";
        public int AdminUserNameMinLength { get; set; }
        public int AdminPasswordMinLength { get; set; } = 6;
        public PasswordRestriction AdminPasswordRestriction { get; set; } = PasswordRestriction.None;
        public bool IsAdminLockLogin { get; set; }
        public int AdminLockLoginCount { get; set; } = 3;
        public LockType AdminLockLoginType { get; set; } = LockType.Hours;
        public int AdminLockLoginHours { get; set; } = 3;
        public bool IsAdminEnforcePasswordChange { get; set; }
        public int AdminEnforcePasswordChangeDays { get; set; } = 90;
        public bool IsAdminEnforceLogout { get; set; }
        public int AdminEnforceLogoutMinutes { get; set; } = 960;
        public bool IsAdminCaptchaDisabled { get; set; }
        public int UserPasswordMinLength { get; set; } = 6;
        public PasswordRestriction UserPasswordRestriction { get; set; } = PasswordRestriction.None;
        public bool IsUserLockLogin { get; set; }
        public int UserLockLoginCount { get; set; } = 3;
        public string UserLockLoginType { get; set; } = "Hours";
        public int UserLockLoginHours { get; set; } = 3;
        public bool IsUserCaptchaDisabled { get; set; }
        public bool IsHomeClosed { get; set; }
        public bool SyncAuto { get; set; } = false;
        public bool DbBackupAuto { get; set; } = false;

        public SystemCode SystemCode { get; set; } = SystemCode.Exam;
        public string SystemCodeName { get; set; } = "星期八在线考试系统";


        //部署配置
        public bool BushuFilesServer { get; set; } = false; //课件服务器单独部署
        public string BushuFilesServerUrl { get; set; } = ""; //课件服务器单独部署

        //积分配置
        public int PointLogin { get; set; } = 5;
        public int PointLoginDayMax { get; set; } = 100;
        public int PointPlanOver { get; set; } = 35;
        public int PointPlanOverDayMax { get; set; } = 100;
        public int PointVideo { get; set; } = 5;
        public int PointVideoDayMax { get; set; } = 100;
        public int PointDocument { get; set; } = 5;
        public int PointDocumentDayMax { get; set; } = 100;
        public int PointCourseOver { get; set; } = 25;
        public int PointCourseOverDayMax { get; set; } = 100;
        public int PointEvaluation { get; set; } = 15;
        public int PointEvaluationDayMax { get; set; } = 100;
        public int PointExam { get; set; } = 10;
        public int PointExamDayMax { get; set; } = 100;
        public int PointExamPass { get; set; } = 15;
        public int PointExamPassDayMax { get; set; } = 100;
        public int PointExamFull { get; set; } = 30;
        public int PointExamFullDayMax { get; set; } = 100;
        public int PointExamQ { get; set; } = 30;
        public int PointExamQDayMax { get; set; } = 100;
        public int PointExamAss { get; set; } = 30;
        public int PointExamAssDayMax { get; set; } = 100;
        public int PointExamPractice { get; set; } = 1;
        public int PointExamPracticeDayMax { get; set; } = 100;
        public int PointExamPracticeRight { get; set; } = 2;
        public int PointExamPracticeRightDayMax { get; set; } = 100;
    }
}
