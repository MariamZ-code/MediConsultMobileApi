using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    public class LabAndScanCentersDTO
    {

     
        public string? provider_name { get; set; }
        public int provider_id { get; set; }

        public List<ServicesDetailsDTO> serviceData { get; set; }

        public decimal? TotalServicePrice { get; set; }

    }


}
