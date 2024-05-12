using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Approval_Act")]
    public class ApprovalAct
    {
        [Key]
        public int Act_id { get; set; }
        public int Act_Approval_id { get; set; }
        public int Act_Qty { get; set; }
    }
}
