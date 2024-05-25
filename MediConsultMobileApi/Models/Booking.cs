using MediConsultMobileApi.Validations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Booking")]
    public class Booking
    {
        public int id { get; set; }
        public string date { get; set; } = DateTime.Now.ToString("dd-MM-yyyy");
        public string time { get; set; } = DateTime.Now.ToString("h:mm:ss tt");
        public string? notes { get; set; }
        public string? status { get; set; } = "Received";
        public string?  attachment { get; set; }
        public string?  member_date { get; set; }

        //[TimeFormat("h:mm:ss tt")]
        public string?  member_time { get; set; } 

        [ForeignKey("member")]

        public int? member_id { get; set; }
        public ClientBranchMember member { get; set; }


        [ForeignKey("ProviderLocation")]
        public int? provider_Location_id { get; set; }
        public ProviderLocation ProviderLocation { get; set; }

        [ForeignKey("Provider")]
        public int? provider_id { get; set; }
        public ProviderData Provider { get; set; }
    }
}
