using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    public class ChangeNationalIdDTO
    {
        public string? national_id { get; set; }
        public int member_id { get; set; }
        public IFormFile? nid_image { get; set; }
    }
}
