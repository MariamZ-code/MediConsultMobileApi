using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.DTO
{
    public class RequestDetailsForMemberDTO
    {
        public int Id { get; set; }
        public string? CreatedDate { get; set; }
        public string ProviderName { get; set; }
        public string? Status { get; set; }
        public string? rejectReason { get; set; }
        public int? ApprovalId { get; set; }
        public string? ApprovalPDF { get; set; }
        public int? is_chronic { get; set; }


    }
}
