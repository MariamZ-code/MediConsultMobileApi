using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("WorkTime")]
    public class WorkTime
    {
        public int id { get; set; }
        public string day { get; set; }

        public string time_from { get; set; }
        public string time_to { get; set; }
        public string time_slot { get; set; }
    }
}
