using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Provider_Location")]
    public class ProviderLocation
    {
        [Key]
        public int location_id { get; set; }
     
        public string location_area_en { get; set; }
        public string location_area_ar { get; set; }
        public string? location_address_en { get; set; }
        public string location_address_ar { get; set; }
        public string? location_telephone_1 { get; set; }
        public string? location_telephone_2 { get; set; }
        public string? location_mobile_1 { get; set; }
        public string? location_mobile_2 { get; set; }
        public string? hotline { get; set; }

        [ForeignKey(nameof(AppSelectorGovernment))]
        public int Location_government_id { get; set; }
        public AppSelectorGovernment AppSelectorGovernment { get; set; }
        //

        [ForeignKey(nameof(Provider))]
        public int provider_id { get; set; }
        public ProviderData Provider { get; set; }

        ///
        [ForeignKey(nameof(AppSelectorGovernmentCity))]

        public int location_city_id { get; set; }
        public AppSelectorGovernmentCity AppSelectorGovernmentCity { get; set; }

    }
}
