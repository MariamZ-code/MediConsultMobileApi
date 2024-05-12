using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.DTO
{
    public class GetNotificationDto
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? body { get; set; }
        public string? image_url { get; set; }
        public int is_seen { get; set; } 
        public int? member_id { get; set; }
        public string? date { get; set; }
        public string? time { get; set; }
        public List<NotificationData> data { get; set; }
    }
}
