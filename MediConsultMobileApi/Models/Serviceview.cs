using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    // TODO : Add To DB
    [Table("Service_view")]
    public class Serviceview
    {
        [Key]
        public int Parent_id { get; set; }
        public int service_id { get; set; }
        public int? hidden { get; set; }
        public string service_name_Ar { get; set; }
        public string service_name_En { get; set; }
        public string Parent_ServiceName_En { get; set; }
        public string Parent_ServiceName_Ar { get; set; }
        public string? photo { get; set; }
        public string? parent_photo { get; set; }
    }
}
