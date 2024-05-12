namespace MediConsultMobileApi.DTO
{

    public class ServiceDataDto
    {

        public string Service_Name_En { get; set; }
        public string? Photo { get; set; }
        public string? copyment { get; set; }
        public int service_id { get; set; }
        public Notes? Notes { get; set; }
    }
    public class Notes
    {
        public List<string>? note { get; set; }=new List<string>();
    }

}
