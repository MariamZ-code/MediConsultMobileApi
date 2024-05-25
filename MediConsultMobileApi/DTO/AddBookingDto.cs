namespace MediConsultMobileApi.DTO
{
    public class AddBookingDto
    {
        public string? notes { get; set; }
        public int? member_id { get; set; }
        public string? date { get; set; }

        public string? time { get; set; }

        public int? provider_id { get; set; }
        public int? provider_Location_id { get; set; }
        public List<IFormFile>? attachment { get; set; }

    }
}
