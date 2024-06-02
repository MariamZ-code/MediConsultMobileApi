using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("GetServicesLabAndScan")]
    [Keyless]
    public class GetServicesLabAndScan
    {
        public int service_id { get; set; }
        public string Service_name_En { get; set; }
        public string Service_name_Ar { get; set; }
    }
}
