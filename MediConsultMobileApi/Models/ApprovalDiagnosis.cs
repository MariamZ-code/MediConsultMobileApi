using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Approval_Diagnosis")]
    public class ApprovalDiagnosis
    {
        [Key]
        [ForeignKey(nameof(Approval))]
        public int Approval_Id { get; set; }
        public Approval Approval { get; set; }

        [ForeignKey(nameof(AppDiagosis))]

        public int Diagnosis_Id { get; set; }

        public AppDiagosis AppDiagosis { get; set; }
    }
}
