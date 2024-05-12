using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("yodawy_medicines_table")]
    public class YodawyMedicins
    {
        [Key]
        public int id { get; set; }
        public string name_en { get; set; }
    }
}
