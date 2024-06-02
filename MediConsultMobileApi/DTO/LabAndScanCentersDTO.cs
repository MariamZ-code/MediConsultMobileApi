using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    public class LabAndScanCentersDTO
    {
   
        public int Service_id { get; set; }
        public string? Service_name { get; set; }
      
        public string? provider_name { get; set; }
        public double? Service_price { get; set; }


    }
}
