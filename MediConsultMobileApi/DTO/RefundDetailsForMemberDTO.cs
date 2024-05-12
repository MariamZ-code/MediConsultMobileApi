namespace MediConsultMobileApi.DTO
{
    public class RefundDetailsForMemberDTO
    {
        public int Id { get; set; }
        public string? CreatedDate { get; set; }
        public string RefundType { get; set; }
        public string? Status { get; set; }
        public string? RefundDate { get; set; }
        public double? Amount { get; set; }
        public string? Note { get; set; }
        public string? refund_reason { get; set; }
        public string? reject_reason { get; set; }
        public string? RefundPDF { get; set; }
        public int? RefundId { get; set; }
        


    }
}
