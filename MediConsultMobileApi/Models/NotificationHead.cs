using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Notification_Head")]
    public class NotificationHead
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? body { get; set; }
        public string? image_url { get; set; }
        public int is_seen { get; set; } = 0;
        public int? member_id { get; set; }
        public string? date { get; set; } = DateTime.Now.ToString("dd-MM-yyyy");
        public string? time { get; set; } = DateTime.Now.ToString("HH:mm");
    }
}
