using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    public class UniqueLabAndScanServiceDto
    {
   
        public int Service_id { get; set; }
        public string? Service_name_En { get; set; }
        public string? Service_Name_Ar { get; set; }
        public string provider_name_en { get; set; }
        public string? provider_name_ar { get; set; }
        public float? Service_price { get; set; }


    }
}
