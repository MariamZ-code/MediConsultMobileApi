using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.DTO
{
    public class MemberFamilyDTO
    {
        public int MemberId { get; set; }

        [MaxLength(300)]
        public string MemberName { get; set; }
        [MaxLength(50)]

        public string? MemberLevel { get; set; }
        [MaxLength(50)]

        public string? MemberBirthday { get; set; }

        [MaxLength(50)]
        public string MemberGender { get; set; }
        public string MemberStatus { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NationalId { get; set; }
        public string? Photo { get; set; }


    }
}
