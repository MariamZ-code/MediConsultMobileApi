namespace MediConsultMobileApi.DTO
{
    public class MemberDetailsProfileArDTO
    {
        public int member_id { get; set; }
        public string member_name { get; set; }
        public string? renew_date { get; set; }
        public string? room_class { get; set; }
        public string? program_name { get; set; }
        public string? member_photo { get; set; }
        public string member_status { get; set; }

        public string? mobile { get; set; }


        public List<MemberECardInfoDTO> notes { get; set; }

        public string? email { get; set; }

        public int? program_id { get; set; }

        public string? member_birthday { get; set; }
        public int? policy_id { get; set; }
        public string? job_title { get; set; }
    }
}
