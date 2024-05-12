using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface INotificationRepository
    {
        NotificationHead GetbyId(int id);
        bool NotificionExists(int id);
        void AddNotification(NotificationMessage notiDTO);
        void UpdateNotification(UpdateNotificationDTO notiDTO, int id);
        void DeleteNotification(int id);
        void AddNotificationFromApproval(NotificationMessageFromApproval notiDTO , Dictionary<string, string> keyValuePairs);
        void AddNotificationFromRefund(NotificationMessageFromReund notiDTO, Dictionary<string, string> keyValuePairs);
        void SendRealTmeNotification(SendRealTmeNotification notiDTO, Dictionary<string, string> keyValuePairs);
        void Save();
        List<NotificationHead> GetBySeen(int memberId);
        List<NotificationHead> GetAll(int memberId);
        List<NotificationData> GetAllData(int notId);
        void DeleteNotificationData(int notiId);
    }
}
