using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.Models
{
    public class AppDiagosis
    {
        [Key]
        public int diagnosis_id { get; set; }

        public string diagnosis_name { get; set; }
        public string daignosis_ICD { get; set; }
        public string is_deleted { get; set; }
    }
}
