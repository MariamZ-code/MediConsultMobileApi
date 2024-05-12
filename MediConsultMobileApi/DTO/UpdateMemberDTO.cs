using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MediConsultMobileApi.DTO
{
    public class UpdateMemberDTO
    {
        
        public string? Mobile { get; set; }
        //public string? SSN { get; set; }
        public string? Email { get; set; }
        public IFormFile? Photo { get; set; }


    }
}
