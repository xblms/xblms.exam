using Datory;
using Datory.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_User")]
    public class User : Entity
    {
        [DataColumn]
        public string UserName { get; set; }

        [DataColumn]
        [JsonIgnore]
        public string Password { get; set; }

        [DataColumn]
        [JsonIgnore]
        public PasswordFormat PasswordFormat { get; set; }

        [DataColumn]
        [JsonIgnore]
        public string PasswordSalt { get; set; }

        [DataColumn]
        public DateTime? LastResetPasswordDate { get; set; }

        [DataColumn]
        public DateTime? LastActivityDate { get; set; }

        [DataColumn]
        public int CountOfLogin { get; set; }

        [DataColumn]
        public int CountOfFailedLogin { get; set; }

        [DataColumn]
        public bool Locked { get; set; }

        [DataColumn]
        public string DisplayName { get; set; }

        [DataColumn]
        public string Mobile { get; set; }

        [DataColumn]
        public string Email { get; set; }

        [DataColumn]
        public string AvatarUrl { get; set; }
        [DataColumn]
        public string AvatarbgUrl { get; set; }
        [DataColumn]
        public string AvatarCerUrl { get; set; }
        [DataColumn]
        public string DutyName { get; set; }
        [DataColumn]
        public int PkRoomId { get; set; }
        [DataColumn]
        public List<string> UserGroupIds { get; set; }
        [DataColumn]
        public string SyncId { get; set; }
        [DataColumn]
        public DateTime? SyncLastActivityDate { get; set; }
        /// <summary>
        /// 全部积分
        /// </summary>
        [DataColumn]
        public int PointsTotal { get; set; }
        /// <summary>
        /// 剩余积分
        /// </summary>
        [DataColumn]
        public int PointsSurplusTotal { get; set; }
        [DataColumn]
        public string PointShopLinkMan { get; set; }
        [DataColumn]
        public string PointShopLinkTel { get; set; }
        [DataColumn]
        public string PointShopLinkAddress { get; set; }
    }
}
