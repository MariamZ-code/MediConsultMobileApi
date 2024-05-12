using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("app_mobile_refund_types")]
    public class RefundType
    {
        [Key]
        public int id { get; set; }

        [MaxLength(400)]
        public string? en_name { get; set; }

        [MaxLength(400)]
        public string? ar_name { get; set; }
    

        // TODO : Add To DB
        public string? notes { get; set; }

      

    }
}
