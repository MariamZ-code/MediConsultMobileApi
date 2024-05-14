using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("approval_timeline_table")]
    public class ApprovalTimeline
    {
        [Key]
        public int id { get; set; }
        public string? action { get; set; }
        public string? portal_source { get; set; }
        public string? datetime { get; set; }
        public int? provider_request_id { get; set; }
        public int? user_id { get; set; }
        public string? status { get; set; }
    }
}
