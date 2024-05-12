using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("App_Sector_Government_City")]
    public class AppSelectorGovernmentCity
    {
        [Key]
        public int city_id { get; set; }
        public string city_name_ar { get; set; }
        public string city_name_en { get; set; }
        public int? is_deleted { get; set; }

        [ForeignKey(nameof(appSelectorGovernment))]
        public int government_id { get; set; }
        public AppSelectorGovernment appSelectorGovernment { get; set; }
    }
}
