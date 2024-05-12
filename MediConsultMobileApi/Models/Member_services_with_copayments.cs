using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Keyless]
    [Table("app_en_get_services_with_copayment_view")]
    public class Member_services_with_copayments
    {
        
        public int program_id { get; set; }
        public int Service_Class_Id { get; set; }
        public string service_nameEn { get; set; }
        public string? service_nameAr { get; set; }
        public int copayment { get; set; }

        // TODO : ADD TO DB
        public string SL_Limit_Type { get; set; }

        // TODO : ADD TO DB
        public int SL_Service_Count { get; set; }
        
        // TODO : ADD TO DB
        public double SL_Limit { get; set; }
        // TODO : ADD TO DB
        public string? notes { get; set; }
        public string? image { get; set; }
    }

}
