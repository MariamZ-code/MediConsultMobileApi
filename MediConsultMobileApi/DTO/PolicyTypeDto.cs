using MediConsultMobileApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    public class PolicyTypeDto
    {

        public int? is_on_program { get; set; }

        public int? program_id { get; set; }

        public string? en_name { get; set; }

        public string? ar_name { get; set; }

    }
}
