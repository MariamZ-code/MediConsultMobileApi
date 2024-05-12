namespace MediConsultMobileApi.DTO
{
    public class UpdateRefundDTO
    {

        public string? notes { get; set; }

        public int? member_id { get; set; }

        public int? refund_type_id { get; set; }

        public double? amount { get; set; }

        public string? refund_date { get; set; }
        public string? refund_reason { get; set; }
        public List<string>? DeletePhotos { get; set; }

        public List<IFormFile>? Photos { get; set; }
    }
}
