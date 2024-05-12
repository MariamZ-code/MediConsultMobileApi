using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("App_General_Sub_Specialty")]
    public class SubGeneralSpecialty
    {
        [Key]
        public int Specialty_Id { get; set; }
        public string Specialty_Name_En { get; set; }
        public string Specialty_Name_Ar { get; set; }
    }
}
