using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Approval_Data")]
    public class Approval
    {
        [Key]
        public int approval_id { get; set; }

        public string approval_status { get; set; }
        public string approval_date { get; set; }
        public int is_canceld { get; set; }
        public int is_chronic { get; set; }
        public int is_repeated { get; set; }
        public string? reject_reason { get; set; }

        [ForeignKey(nameof(Provider))]
        public int provider_id { get; set; }

        public ProviderData Provider { get; set; }

        [ForeignKey(nameof(ClientBranchMember))]
        public int member_id { get; set; }

        public ClientBranchMember ClientBranchMember { get; set; }

    }
}
