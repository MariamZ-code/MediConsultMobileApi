using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    // TODO : 
    [Table("Member_NationalId")]
    public class MemberNationalId
    {
        [Key]
        public int id { get; set; }

        [MaxLength(20)]
        public string national_id { get; set; }

        public int member_id { get; set; }

        [MaxLength(150)]

        public string nid_image { get; set; }
    }
}
