using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.DTO
{
    public class BookingLabAndScanCenterDTO
    {
        public int? provider_id { get; set; }
        public int? member_id { get; set; }
        public ICollection<int>? serviceIds { get; set; }

    }
}
