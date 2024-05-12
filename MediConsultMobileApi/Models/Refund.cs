using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("client_portal_refund_request_table")]
    public class Refund
    {
        [Key]
        public int id { get; set; }


        public string? notes { get; set; }

        [MaxLength(50)]
        public string? created_date { get; set; } = DateTime.Now.ToString("dd-MM-yyyy");

        public string? refund_date { get; set; } = DateTime.Now.ToString("dd-MM-yyyy");
        public double? total_amount { get; set; }

        public int? Provider_location_id { get; set; } = 0;

        public int? member_id { get; set; }

        [MaxLength(100)]
        public string? Status { get; set; } = "Received";

        [ForeignKey("refundTypes")]
        public int? refund_type_id { get; set; }
        public virtual RefundType? refundTypes { get; set; }

        public int? Is_pharma { get; set; } = 0;
        public string? reject_reason { get; set; }

        public string? folder_path { get; set; }

        [MaxLength(50)]
        public string? the_email { get; set; }

        public int? Is_notified { get; set; } = 0;

        public int? Client_user_id { get; set; } = 0;
        public int? Mc_notified { get; set; } = 0;

        [MaxLength(50)]
        public string? whatsapp_number { get; set; }

        public int? refund_id { get; set; } = 0 ;

    
        public string? refund_reason { get; set; }
    


    }
}
