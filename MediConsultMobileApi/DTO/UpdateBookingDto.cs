namespace MediConsultMobileApi.DTO
{
    public class UpdateBookingDto
    {
        public string? notes { get; set; }
        public int? member_id { get; set; }

        public List<IFormFile>? attachment { get; set; }

        public int? provider_id { get; set; }
        public List<string>? DeletePhotos { get; set; }
        public int? provider_Location_id { get; set; }
    }
}
