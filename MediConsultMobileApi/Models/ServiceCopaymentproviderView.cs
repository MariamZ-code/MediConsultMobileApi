using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    //TODO : Add to db
    [Table("Service_Copayment_provider_View")]
    public class ServiceCopaymentproviderView
    {
        [Key]
        public int member_id { get; set; }
        public string? provider_name_en { get; set; }
        public string? provider_name_ar { get; set; }
        public string? notes { get; set; }
        public double? copayment_percent { get; set; }
        public int Service_Class_id { get; set; }
        public double SL_Limit { get; set; }
        public string SL_Limit_Type { get; set; }
        public int SL_Service_Count { get; set; }
        public int? Service_Class_Parent_Id { get; set; }
    }
}
