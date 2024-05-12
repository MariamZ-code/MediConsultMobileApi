using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MediConsultMobileApi.Models
{
    [Table("Client_Branch_Member")]
    public class ClientBranchMember
    {
        [Key]
        public int member_id { get; set; }

        [MaxLength(300)]
        public string member_name { get; set; }

        [MaxLength(50)]

        public string? member_level { get; set; }

        [MaxLength(50)]

        public string? member_birthday { get; set; }

        [MaxLength(50)]
        [RegularExpression(@"^01[1235][0-9]{8}$", ErrorMessage = "Invalid phone number, please enter valid phone number")]
        public string? mobile { get; set; }
        public string member_gender { get; set; }
        public string member_status { get; set; }
        public string? email { get; set; }

        [MaxLength(500)]
        public string? member_photo { get; set; }

        [MaxLength(50)]
        public string? member_nid { get; set; }

        [JsonIgnore]
        public int? member_HOF_id { get; set; }

        // TODO : Add Column to DB

        public string? job_title { get; set; }

        // TODO : Add Column to DB
        public string? the_password { get; set; }

        // TODO : Add Column to DB
        public int? is_enabled { get; set; }

        // TODO : Add Column to DB
        public string? firebase_token { get; set; }

        // TODO : Add Column to DB
        public string? Otp { get; set; }
    }
}
