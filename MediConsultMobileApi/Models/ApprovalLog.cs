using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("approval_log_table")]
    public class ApprovalLog
    {
        public int id { get; set; }
        public int claim_id { get; set; }
        public string _event { get; set; }
        public string user_id { get; set; }
        public string the_date { get; set; } = DateTime.Now.ToString("dd-MM-yyyy");
        public string the_time { get; set; } = DateTime.Now.ToString("h:mm:ss tt");
    }
}
