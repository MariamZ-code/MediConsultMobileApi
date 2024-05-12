using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("App_General_Specialty")]
    public class GeneralSpecialty
    {
        [Key]
        public int General_Specialty_Id { get; set; }
        public string General_Specialty_Name_En { get; set; }
        public string General_Specialty_Name_Ar { get; set; }
    }
}
