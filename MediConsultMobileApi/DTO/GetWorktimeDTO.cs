namespace MediConsultMobileApi.DTO
{
    public class GetWorktimeDTO
    {
        public int Rrovider_id { get; set; }
        public int Worktime_id { get; set; }
        public string Day { get; set; }
        public string Time_From { get; set; }
        public string Time_To { get; set; }
        public int Slot { get; set; }
    }
}
