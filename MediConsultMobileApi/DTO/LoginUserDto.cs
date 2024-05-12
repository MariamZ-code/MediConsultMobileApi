using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MediConsultMobileApi.DTO
{
    
    public class LoginUserDto
    {
      
        public string Id { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public string? Firebase_token { get; set; }

    }
}
