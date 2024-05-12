using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("App_Sector_Government")]
    public class AppSelectorGovernment
    {
        [Key]
        public int government_id { get; set; }
        public string government_name_ar { get; set; }
        public string government_name_en { get; set; }
        public int is_deleted { get; set; }
    }
}
