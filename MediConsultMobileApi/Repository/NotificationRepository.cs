using FirebaseAdmin.Messaging;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediConsultMobileApi.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext dbContext;

        public NotificationRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public NotificationHead GetbyId(int id)
        {
            return dbContext.NotificationHeads.FirstOrDefault(n => n.id == id);
        }

        #region AddNotification
        public void AddNotification(NotificationMessage notiDTO)
        {
            foreach (var member in notiDTO.membersIds)
            {
                var newNoti = new NotificationHead()
                {
                    member_id = int.Parse(member),
                    title = notiDTO.Title,
                    body = notiDTO.Body,
                    image_url = notiDTO.ImageUrl
                };

                dbContext.NotificationHeads.Add(newNoti);
            }

        }

        #endregion

        #region SendRealTmeNotification
        public void SendRealTmeNotification(SendRealTmeNotification notiDTO, Dictionary<string, string> keyValuePairs)
        {
            
            foreach (var memberId in notiDTO.membersId)
            {
                var newNoti = new NotificationHead()
                {
                    member_id = int.Parse(memberId),
                    title = notiDTO.Title,
                    body = notiDTO.Body,
                    image_url = notiDTO.ImageUrl
                };
              

                dbContext.NotificationHeads.Add(newNoti);
                var member = dbContext.clientBranchMembers.FirstOrDefault(m => m.member_id ==int.Parse(memberId));
                member.is_enabled = notiDTO.is_enabled;
                dbContext.clientBranchMembers.Update(member);
                Save();
                foreach (var kv in keyValuePairs)
                {
                    var newDataNoti = new NotificationData()
                    {
                        notificationHead_Id = newNoti.id,
                        key_data = kv.Key,
                        value = kv.Value,
                    };

                    dbContext.NotificationDatas.Add(newDataNoti);
                }
            }

        }

        #endregion


        #region AddNotificationFromApproval
        public void AddNotificationFromApproval(NotificationMessageFromApproval notiDTO, Dictionary<string, string> keyValuePairs)
        {
            foreach (var memberId in notiDTO.membersId)
            {
                var newNoti = new NotificationHead()
                {
                    member_id = int.Parse(memberId),
                    title = notiDTO.Title,
                    body = notiDTO.Body,
                    image_url = notiDTO.ImageUrl
                };

                dbContext.NotificationHeads.Add(newNoti);
                Save();
                foreach (var kv in keyValuePairs)
                {
                    var newDataNoti = new NotificationData()
                    {
                        notificationHead_Id = newNoti.id,
                        key_data = kv.Key,
                        value = kv.Value,
                    };

                    dbContext.NotificationDatas.Add(newDataNoti);
                }
            }

        }

        #endregion

        #region AddNotificationFromRefund
        public void AddNotificationFromRefund(NotificationMessageFromReund notiDTO, Dictionary<string, string> keyValuePairs)
        {
            foreach (var memberId in notiDTO.membersId)
            {
                var newNoti = new NotificationHead()
                {
                    member_id = int.Parse(memberId),
                    title = notiDTO.Title,
                    body = notiDTO.Body,
                    image_url = notiDTO.ImageUrl
                };

                dbContext.NotificationHeads.Add(newNoti);
                Save();
                foreach (var kv in keyValuePairs)
                {
                    var newDataNoti = new NotificationData()
                    {
                        notificationHead_Id = newNoti.id,
                        key_data = kv.Key,
                        value = kv.Value,
                    };

                    dbContext.NotificationDatas.Add(newDataNoti);
                }
            }

        }

        #endregion

        #region DeleteNotification
        public void DeleteNotification(int id)
        {
            var notification = GetbyId(id);
            dbContext.NotificationHeads.Remove(notification);
        }
        public void DeleteNotificationData(int notiId)
        {
            var datas = dbContext.NotificationDatas.Where(n => n.notificationHead_Id == notiId).AsNoTracking().ToList();
            foreach (var data in datas)
            {

                dbContext.NotificationDatas.Remove(data);
            }
        }

        #endregion


        #region Save
        public void Save()
        {
            dbContext.SaveChanges();
        }

        #endregion


        #region UpdateNotification
        public void UpdateNotification(UpdateNotificationDTO notiDTO, int id)
        {
            var notification = GetbyId(id);
            notification.is_seen = notiDTO.is_seen;

            dbContext.NotificationHeads.Update(notification);

        }
        #endregion

        #region NotificationExists
        public bool NotificionExists(int id)
        {
            return dbContext.NotificationHeads.Any(x => x.id == id);
        }
        #endregion

        #region CountNotification

        public List<NotificationHead> GetBySeen(int memberId)
        {
            return dbContext.NotificationHeads.Where(n => n.is_seen == 0 && n.member_id == memberId).ToList();
        }
        #endregion

        #region GetAll
        public List<NotificationHead> GetAll(int memberId)
        {
            return dbContext.NotificationHeads.Where(m => m.member_id == memberId).AsNoTracking().ToList();
        }
        public List<NotificationData> GetAllData(int notId)
        {
            return dbContext.NotificationDatas.Where(m => m.notificationHead_Id == notId).AsNoTracking().ToList();
        }


        #endregion
    }
}
