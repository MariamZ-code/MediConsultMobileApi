using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Service_Data")]
    public class ServiceData
    {
        [Key]
        public int Service_id { get; set; }
        public string Service_name_En { get; set; }
        public string Service_Name_Ar { get; set; }
        public string Service_Status { get; set; }

        public ICollection<BookingService> BookingServices { get; set; } 

    }
}
