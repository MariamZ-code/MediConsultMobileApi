using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.DTO
{
    public class MedicalNetworkDTO
    {
        public int TotalCount { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public MedicalNetwork MedicalNetwork { get; set; }
    }
}
