using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("app_en_get_member_info_view")]
    public class Member
    {
        [Key]
        public int member_id { get; set; }
        public string member_name { get; set; }
        public string? renew_date { get; set; }
        public string? room_class { get; set; }
        public string? program_name { get; set; }
        public string? member_photo { get; set; }
        public string member_status { get; set; }

        public string? mobile { get; set; }

        public string? Type_Name_Ar { get; set; }
        public string? Type_Name_En { get; set; }

        public string? email { get; set; }

        public int? program_id { get; set; }

        public int? policy_id { get; set; }
        public string? job_title { get; set; }

        // TODO : Add Column to DB
        public string? member_birthday { get; set; }
 


    }
}
