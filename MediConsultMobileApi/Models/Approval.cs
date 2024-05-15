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
        public string approval_validation_period { get; set; }
        public int client_id { get; set; }
        public int Client_Branch_id { get; set; }
        public int policy_id { get; set; }
        public int program_id { get; set; }

        public int service_class_id { get; set; }
        public int provider_location_id { get; set; }

        public int Approval_User_Id { get; set; }
        public int Approval_Force_Debit { get; set; }
        public int? price_list_id { get; set; }
        public string? internal_notes { get; set; }
        public int? is_claimed { get; set; }
        public int? general_specality_id { get; set; }
        public int? pool_child_id { get; set; }
        public int is_canceld { get; set; }
        public int? is_re_auth { get; set; }
        public double? pool_spent { get; set; }
        public int? exceed_pool_id { get; set; }

        public double money_for_exceed_note { get; set; }
        public int? is_pharma { get; set; }
        public int is_chronic { get; set; }
        public string? claim_form_no { get; set; }

        public double debit_spent { get; set; }
        public int? is_repeated { get; set; }
        public string? cancel_reason { get; set; }
        public int? exception_child_id { get; set; }

        public int? inpatient_duration_days { get; set; }
        public string? inpatient_duration_type { get; set; }
        public int? additional_pool_id { get; set; }
        public int? doctor_id { get; set; }
        public string? dental_comment { get; set; }


        public string? reject_reason { get; set; }
        public int? is_critical { get; set; }
        public int? is_exception { get; set; }


        [ForeignKey(nameof(Provider))]
        public int provider_id { get; set; }

        public ProviderData Provider { get; set; }

        [ForeignKey(nameof(ClientBranchMember))]
        public int member_id { get; set; }

        public ClientBranchMember ClientBranchMember { get; set; }


    }
}
