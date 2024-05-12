using FirebaseAdmin.Messaging;
using MediConsultMobileApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.HangFire
{
    public class HangFireController
    {
        private readonly ApplicationDbContext dbContext;


        public HangFireController(ApplicationDbContext dbContext)
        {

            this.dbContext = dbContext;

        }

        #region SendNotification


        public  void SendNotificationHangFire(string lang)
        {

            Console.WriteLine($"send message {DateTime.Now}");
            //var userIds = notificationMessage.membersIds.Select(id => id.ToString()).ToList();

            //var firebaseTokens =  dbContext.clientBranchMembers
            //    .Where(x => userIds.Contains(x.member_id.ToString()) && x.firebase_token != null)
            //    .Select(x => x.firebase_token)
            //    .ToList();
            //if (notificationMessage.ImageUrl == string.Empty)
            //{
            //    var message = new MulticastMessage
            //    {
            //        Tokens = firebaseTokens,
            //        Notification = new Notification
            //        {
            //            Title = notificationMessage.Title,
            //            Body = notificationMessage.Body,
            //        },
            //    };

            //    var response =  FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);

            //}

            //var messa = new MulticastMessage()
            //{
            //    Tokens = firebaseTokens,
            //    Notification = new Notification
            //    {
            //        Title = notificationMessage.Title,
            //        Body = notificationMessage.Body,
            //        ImageUrl = notificationMessage.ImageUrl
            //    }
            //};

            //var respo =  FirebaseMessaging.DefaultInstance.SendMulticastAsync(messa);


        }
        private bool IsUrlValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        #endregion
    }
}
