using Datory;
using Datory.Annotations;
using Newtonsoft.Json;
using System;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("xblms_Administrator")]
    public class Administrator : Entity
    {
        [DataColumn]
        public AuthorityType Auth { get; set; }
        [DataColumn]
        public AuthorityDataType AuthData { get; set; }
        /// <summary>
        /// 显示组织下所有数据还是显示当前组织下的数据
        /// </summary>
        [DataColumn]
        public bool AuthDataShowAll { get; set; } = true;
        [DataColumn]
        public int AuthDataCurrentOrganId { get; set; }
        [DataColumn]
        public string UserName { get; set; }

        [JsonIgnore]
        [DataColumn]
        public string Password { get; set; }

        [JsonIgnore]
        [DataColumn]
        public PasswordFormat PasswordFormat { get; set; }

        [JsonIgnore]
        [DataColumn]
        public string PasswordSalt { get; set; }

        [DataColumn]
        public DateTime? LastActivityDate { get; set; }

        [DataColumn]
        public DateTime? LastChangePasswordDate { get; set; }

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
    }
}
