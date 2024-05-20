using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("portal_users_table")]
    public class PortalUsersTable
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(Provider))]
        public int? provider_id { get; set; }
        
        public ProviderData? Provider { get; set; }


        public int? is_enabled { get; set; }
    }
}
