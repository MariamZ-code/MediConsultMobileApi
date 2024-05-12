using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    public class MemberDetailsDTO
    {
        public int member_id { get; set; }

        public string member_name { get; set; }
        public string? mobile { get; set; }
        public string? member_nid { get; set; }
        public string member_gender { get; set; }
        public string? email { get; set; }
        public string? member_photo { get; set; }
        public string? birthDate { get; set; }
        public string? jobTitle { get; set; }
        public object? image2 { get; set; }
    }
}
