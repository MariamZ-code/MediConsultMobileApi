using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Provider_Data")]
    //approval_id
    public class ProviderData
    {
        [Key]
        public int provider_id { get; set; }

        [Required]
        [StringLength(200)]  
        public string provider_name_en { get; set; }

        [StringLength(200)]

        public string provider_name_ar { get; set; }

        [StringLength(50)]
        public string provider_oid { get; set; }
        public string? provider_commercial_name { get; set; }
        public string? provider_short_number { get; set; }
        public string provider_status { get; set; }

        [ForeignKey(nameof(Category))]
        public int Category_ID { get; set; }
        public Category Category { get; set; }

        //public List<Request> Requests { get; set; }
    }
}
