using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository notificationRepo;

        public NotificationController(INotificationRepository notificationRepo)
        {
            this.notificationRepo = notificationRepo;
        }
        #region AllNotification

        [HttpGet("AllNotification")]
        public IActionResult GetAll(int memberId)
        {
            if (ModelState.IsValid)
            {
                var notiList = new List<GetNotificationDto>();
                var allNotifications = notificationRepo.GetAll(memberId);
                foreach (var notification in allNotifications)
                {
                    var allDataNotifications = notificationRepo.GetAllData(notification.id);
                   
                    var noti = new GetNotificationDto
                        {
                            title = notification.title,
                            body = notification.body,
                            id = notification.id,
                            is_seen = notification.is_seen,
                            member_id = memberId,
                            date = notification.date,
                            time = notification.time,
                            image_url = notification.image_url,
                            data = allDataNotifications
                    };
                    notiList.Add(noti);
                }
                return Ok(notiList);

            }
            return BadRequest(ModelState);
        }

        #endregion

        #region SeenNotification
        [HttpGet("SeenNotification")]
        public IActionResult Get(int memberId)
        {
            if (ModelState.IsValid)
            {
                var seenNoti = notificationRepo.GetBySeen(memberId);

                return Ok(new MessageDto { Message = seenNoti.Count.ToString() });
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region UpdateNotification
        [HttpPost("UpdateNotification")]
        public IActionResult Update(UpdateNotificationDTO notiDTO, int id)
        {
            if (ModelState.IsValid)
            {
                var notification = notificationRepo.NotificionExists(id);
                if (!notification)
                {
                    return NotFound(new MessageDto { Message= "Notification Not found " });
                }
                if (notiDTO.is_seen != 1)
                {
                    return BadRequest(new MessageDto { Message = "Error" });

                }
                notificationRepo.UpdateNotification(notiDTO, id);
                notificationRepo.Save();
                return Ok(new MessageDto { Message = "Updated" });

            }
            return BadRequest(ModelState);
        }

        #endregion

        #region Delete
        [HttpPost("Delete")]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var notification = notificationRepo.NotificionExists(id);
                if (!notification)
                {
                    return NotFound(new MessageDto { Message = "Notification Not found " });
                }
                var notii = notificationRepo.GetbyId(id);
                notificationRepo.DeleteNotificationData(notii.id);
                notificationRepo.Save();

                notificationRepo.DeleteNotification(id);

                notificationRepo.Save();
                return Ok(new MessageDto { Message= "Deleted"});
            }
            return BadRequest(ModelState);
        }

        #endregion
    }
}
