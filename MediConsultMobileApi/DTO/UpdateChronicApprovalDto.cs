namespace MediConsultMobileApi.DTO
{
    public class UpdateChronicApprovalDto
    {
        public string? Notes { get; set; }
        public int? Member_id { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public List<string>? DeletePhotos { get; set; }
    }
}
