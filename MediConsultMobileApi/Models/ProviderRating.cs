using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Provider_Rating")]
    public class ProviderRating
    {
        [Key]
        public int rate_id { get; set; }

        public int? rate { get; set; }

        [ForeignKey("ProviderData")]
        public int ?provider_id { get; set; }
        public ProviderData ProviderData { get; set; }

        [ForeignKey("Member")]
        public int? member_id { get; set; }
        public ClientBranchMember Member { get; set; }
    }
}
