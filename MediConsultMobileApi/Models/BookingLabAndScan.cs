using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    //TODO : Add to db
    
    [Table("BookingLabAndScan")]
    public class BookingLabAndScan
    {
        public int id { get; set; }

        [ForeignKey(nameof(provider))]
        public int? provider_id { get; set; }

        public ProviderData provider { get; set; }

        [ForeignKey(nameof(member))]

        public int? member_id { get; set; }
        public ClientBranchMember member { get; set; }


        public ICollection<BookingService> service { get; set; } 

        public string date { get; set; } = DateTime.Now.ToString("dd-MM-yyyy");
        public string status { get; set; } = "Received";

    }
}
