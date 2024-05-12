using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Provider_Specialty")]
    public class ProviderSpecialty
    {
        
        //public int id { get; set; }

        [ForeignKey(nameof(Provider))]
        [Key]
        public int provider_id { get; set; }
        public ProviderData Provider { get; set; }

        [ForeignKey(nameof(GeneralSpecialty))]
        public int General_specialty_Id { get; set; }
        public GeneralSpecialty GeneralSpecialty { get; set; }

        [ForeignKey(nameof(subGeneralSpecialty))]
        public int Specialty_Id { get; set; }
        public SubGeneralSpecialty subGeneralSpecialty { get; set; }


    }
}
