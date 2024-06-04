using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    public class LabAndScanCentersDTO
    {

     
        public string? ProviderName { get; set; }
        public int ProviderId { get; set; }

        public decimal? TotalServicePrice { get; set; }

        public List<string> ServiceIdNotInList { get; set; }
        public List<ServicesDetailsDTO> ServiceData { get; set; }


    }


}
