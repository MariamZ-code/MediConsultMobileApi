using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("app_act_reasons_table")]
    public class AppActReasons
    {
        [Key]
        public int id { get; set; }
        public  string reason { get; set; }
    }
}
