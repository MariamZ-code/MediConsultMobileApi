using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("LabAndScanCenter")]
    //[Keyless]
    public class LabAndScanCenter
    {
        [Key]
        public int Service_id { get; set; }
        public string? Service_name_En { get; set; }
        public string? Service_Name_Ar { get; set; }
        public string provider_name_en { get; set; }
        public string? provider_name_ar { get; set; }
        public double? Service_price { get; set; }
    }
}
