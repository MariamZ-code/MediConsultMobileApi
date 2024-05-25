using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Member_Provider_Favorite")]
    public class MemberProviderFavorite
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("ProviderData")]
        public int provider_id { get; set; }
        public ProviderData ProviderData { get; set; }

        [ForeignKey("Member")]
        public int member_id { get; set; }
        public ClientBranchMember Member { get; set; }
    }
}
