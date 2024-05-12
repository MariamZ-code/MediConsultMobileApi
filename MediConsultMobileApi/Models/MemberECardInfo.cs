using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    // TODO : Add to DB
    [Table("Member_ECardInfo")]
    public class MemberECardInfo
    {
        public int id { get; set; }
        public int? program_id { get; set; } 

        public string? notes { get; set; }
    }
}
