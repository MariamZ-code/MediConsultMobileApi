using MediConsultMobileApi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.DTO
{
    public class ProgramInfoDTO
    {
        public int? program_id { get; set; }

        public int? service_id { get; set; }
     
        public string info { get; set; }
    }
}
