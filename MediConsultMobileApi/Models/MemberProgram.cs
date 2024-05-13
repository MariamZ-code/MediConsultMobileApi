using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Member_Program")]
    public class MemberProgram
    {
        [Key]
        public int Client_ID { get; set; }
        public int Member_Id { get; set; }
        public int Policy_Id { get; set; }
        public int Program_id { get; set; }
        public string add_date { get; set; }
    }
}
