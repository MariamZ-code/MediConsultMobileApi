using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("pharma_approval_act")]
    public class PharmaApprovalAct
    {
        [Key]
        public int id { get; set; }
        public double Act_Qty { get; set; }
        public decimal Act_Price { get; set; }
        public decimal Act_Discount { get; set; }
        public decimal Act_Discount_Value { get; set; }
        public decimal Act_Copayment_Percentage { get; set; }
        public decimal Act_Copayment_Value { get; set; }
        public decimal Act_Total_Amount { get; set; }
        public decimal Act_Grand_Total { get; set; }
        public string? dose { get; set; }
        public string? unit_name { get; set; }
        public string Act_Status { get; set; }


        // TODO : Allow Null

        [ForeignKey(nameof(AppActReasons))]
        public int? Act_Status_Reason { get; set; }
        public AppActReasons AppActReasons { get; set; }


        [ForeignKey(nameof(Approval))]
        public int Act_Approval_id { get; set; }

        public Approval Approval { get; set; }


        [ForeignKey(nameof(YodawyMedicins))]
        public int Act_id { get; set; }
        public YodawyMedicins YodawyMedicins { get; set; }
    }
}
