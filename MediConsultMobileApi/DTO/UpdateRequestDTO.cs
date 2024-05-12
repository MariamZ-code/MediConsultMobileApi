using System.Text.Json.Serialization;

namespace MediConsultMobileApi.DTO
{
    public class UpdateRequestDTO
    {
        public string? Notes { get; set; }
        public int? Provider_id { get; set; }
        public int? Member_id { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public List<string>? DeletePhotos { get; set; }

    }
}
