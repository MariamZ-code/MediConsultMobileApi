namespace MediConsultMobileApi.Models
{
    public class NotificationMessage
    {
        public List<string>? membersIds { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ImageUrl { get; set; }


    }
    // SendRealTmeNotification

    public class NotificationMessageToAll
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ImageUrl { get; set; }
    }
    public class NotificationMessageFromApproval
    {
        public List<string>? membersId { get; set; }

        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ImageUrl { get; set; }
        public int request_id { get; set; }
        public int approval_id { get; set; }
        public string is_approved { get; set; }
    }

    public class SendRealTmeNotification
    {
        public List<string>? membersId { get; set; }

        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ImageUrl { get; set; }
        public int is_enabled { get; set; }
     
    }

    public class NotificationMessageFromReund
    {
        public List<string>? membersId { get; set; }

        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ImageUrl { get; set; }
        public int refund_id { get; set; }
        public int request_id { get; set; }
        public string is_refund { get; set; }
    }
}
