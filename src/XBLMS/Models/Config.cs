using System;
using System.Collections.Generic;
using Datory;
using Datory.Annotations;
using XBLMS.Configuration;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_Config")]
    public class Config : Entity
    {
        [DataColumn]
        public string DatabaseVersion { get; set; }

        [DataColumn]
        public DateTime UpdateDate { get; set; }
        public bool Initialized => Id > 0;
        public bool IsLogAdmin { get; set; } = true;
        public bool IsLogUser { get; set; } = true;
        public bool IsLogError { get; set; } = true;
        public bool IsTimeThreshold { get; set; }
        public int TimeThreshold { get; set; } = 60;
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

    }
}
