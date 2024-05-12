using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Notification_Data")]
    public class NotificationData
    {
        public int id { get; set; }

        [JsonIgnore]

        [ForeignKey("Notification")]
        public int notificationHead_Id { get; set; }

        [JsonIgnore]
        public virtual NotificationHead Notification { get; set; }
        public string key_data { get; set; }
        public string value { get; set; }
    }
}
