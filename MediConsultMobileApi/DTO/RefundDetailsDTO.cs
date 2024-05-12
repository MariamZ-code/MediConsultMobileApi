using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.DTO
{
    public class RefundDetailsDTO
    {
        public int Id { get; set; }
        public int? RefundId { get; set; }
        public string RefundType { get; set; }
        public string? Refund_date { get; set; }

        public string? reject_reason { get; set; }
        public bool Allow_Edit { get; set; }

        public List<string> FolderPath { get; set; }
        public double? Amount { get; set; }
        public string? Notes { get; set; }
        public string? refund_reason { get; set; }


    }
}
