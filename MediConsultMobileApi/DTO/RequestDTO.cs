using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MediConsultMobileApi.DTO
{
    public class RequestDTO
    {
       
        public string? Notes { get; set; }

       
        public int? Provider_id { get; set; }

        [JsonIgnore]
        public int? Member_id { get; set; }



       
    }
}
