namespace MediConsultMobileApi.DTO
{
    public class BookingDetailsDTO
    {
        public int bookingId { get; set; }

        public string? providerName { get; set; }
        public string? govName { get; set; }
        public string? cityName { get; set; }

        public string? areaName { get; set; }
        public string? address { get; set; }
        public string? specialtyName { get; set; }
        public string? subSpecialtyName { get; set; }
        public string? categoryName { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string? notes { get; set; }
        public List<string> folderPath { get; set; }

        public string? status { get; set; }

    }
}
