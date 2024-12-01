using Datory;
using Datory.Annotations;
using Newtonsoft.Json;
using System;
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
        public int DutyId { get; set; }
        [DataColumn]
        public int PkRoomId { get; set; }
    }
}
