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
        public string unit2_name { get; set; }
        public double unit2_count { get; set; }
        public double sell_price { get; set; }

        public string full_form { get; set; }
    
    }
}
