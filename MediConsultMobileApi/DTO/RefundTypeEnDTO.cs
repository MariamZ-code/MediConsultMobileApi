using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    public class RefundTypeEnDTO
    {
        public int id { get; set; }
        
        public string? en_name { get; set; }
        public string? notes { get; set; }

    }
}
