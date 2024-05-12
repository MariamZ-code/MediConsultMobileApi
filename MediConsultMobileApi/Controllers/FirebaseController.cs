using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.HangFire;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;
        private readonly INotificationRepository notiRepo;
        private readonly HangFireController hangFire;

        public FirebaseController(ApplicationDbContext dbContext, INotificationRepository notiRepo , HangFireController hangFire)
        {

            this.dbContext = dbContext;
            this.notiRepo = notiRepo;
            this.hangFire = hangFire;
        }

        #region SendNotification

        [HttpPost("SendNotification")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationMessage notificationMessage, string lang)
        {
           
            if (notificationMessage == null)
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationValid(lang) });

            }

            var userIds = notificationMessage.membersIds.Select(id => id.ToString()).ToList();

            var firebaseTokens = await dbContext.clientBranchMembers
                .Where(x => userIds.Contains(x.member_id.ToString()) && x.firebase_token != null)
                .Select(x => x.firebase_token)
                .ToListAsync();

            if (firebaseTokens.Count == 0)
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationToken(lang) });

            }
            if (notificationMessage.ImageUrl == string.Empty)
            {
                var message = new MulticastMessage
                {
                    Tokens = firebaseTokens,
                    Notification = new Notification
                    {
                        Title = notificationMessage.Title,
                        Body = notificationMessage.Body,
                    },
                };

                notiRepo.AddNotification(notificationMessage);
                notiRepo.Save();
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });


            }
            if (!IsUrlValid(notificationMessage.ImageUrl))
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationImage(lang) });


            }

            var messa = new MulticastMessage()
            {
                Tokens = firebaseTokens,
                Notification = new Notification
                {
                    Title = notificationMessage.Title,
                    Body = notificationMessage.Body,
                    ImageUrl = notificationMessage.ImageUrl
                }
            };

            notiRepo.AddNotification(notificationMessage);
            notiRepo.Save();
            var respo = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(messa);


            return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });



        }
        private bool IsUrlValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        #endregion

        #region SendNotificationToAll
        [Route("SendNotificationToAll")]
        [HttpPost]
        public async Task<IActionResult> SendNotificationToAll([Required][FromBody] NotificationMessageToAll notificationMessage, string lang)
        {

            if (notificationMessage.ImageUrl == string.Empty)
            {

                var message = new Message
                {
                    Notification = new Notification
                    {
                        Title = notificationMessage.Title,
                        Body = notificationMessage.Body,

                    },
                    Topic = "all"
                };

                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });



            }
            if (!IsUrlValid(notificationMessage.ImageUrl))
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationImage(lang) });

            }
            var messa = new Message
            {

                Notification = new Notification
                {
                    Title = notificationMessage.Title,
                    Body = notificationMessage.Body,
                    ImageUrl = notificationMessage.ImageUrl
                },
                Topic = "all"

            };

            var respo = await FirebaseMessaging.DefaultInstance.SendAsync(messa);

            return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });


        }



        #endregion


        #region SendApprovalNotification

        [HttpPost("SendApprovalNotification")]
        public async Task<IActionResult> SendApprovalNotification([FromBody] NotificationMessageFromApproval notificationMessage, string lang)
        {
            if (notificationMessage == null)
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationValid(lang) });

            }
            var userIds = notificationMessage.membersId.Select(id => id.ToString()).ToList();

            var firebaseTokens = await dbContext.clientBranchMembers
                .Where(x => userIds.Contains(x.member_id.ToString()) && x.firebase_token != null)
                .Select(x => x.firebase_token)
                .ToListAsync();

            if (firebaseTokens.Count == 0)
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationToken(lang) });

            }
            var customData = new Dictionary<string, string>
            {
                { "request_id", notificationMessage.request_id.ToString() },
                { "approval_id", notificationMessage.approval_id.ToString() },
                { "is_approved", notificationMessage.is_approved },

            };

            if (notificationMessage.ImageUrl == string.Empty)
            {
                var message = new MulticastMessage
                {
                    Data = customData,
                    Tokens = firebaseTokens,
                    Notification = new Notification
                    {
                        Title = notificationMessage.Title,
                        Body = notificationMessage.Body,

                    },
                };
                string json = JsonConvert.SerializeObject(message);
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                notiRepo.AddNotificationFromApproval(notificationMessage, customData);
                notiRepo.Save();
                return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });


            }
            if (!IsUrlValid(notificationMessage.ImageUrl))
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationImage(lang) });


            }

           
            var messa = new MulticastMessage
            {
                Data = customData,
                Tokens = firebaseTokens,
                Notification = new Notification
                {
                    Title = notificationMessage.Title,
                    Body = notificationMessage.Body,
                    ImageUrl = notificationMessage.ImageUrl
                }
            };
            var respo = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(messa);
            notiRepo.AddNotificationFromApproval(notificationMessage, customData);
            notiRepo.Save();
            return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });

        }

        //public static string EscapeNewlines(string jsonString)
        //{
        //    // Replace newline character with its escaped version
        //    return jsonString.Replace("\n", "\\n");
        //}
        #endregion

        #region SendRealTmeNotification

        [HttpPost("SendRealTmeNotification")]
        public async Task<IActionResult> SendRealTmeNotification([FromBody] SendRealTmeNotification notificationMessage, string lang)
        {

            if (notificationMessage == null)
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationValid(lang) });

            }

            var userIds = notificationMessage.membersId.Select(id => id.ToString()).ToList();

            var firebaseTokens = await dbContext.clientBranchMembers
                .Where(x => userIds.Contains(x.member_id.ToString()) && x.firebase_token != null)
                .Select(x => x.firebase_token)
                .ToListAsync();

            if (firebaseTokens.Count == 0)
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationToken(lang) });

            }
            
            var customData = new Dictionary<string, string>
            {
              
                { "is_enabled", notificationMessage.is_enabled.ToString()},

            };

            if (notificationMessage.ImageUrl == string.Empty)
            {
                var message = new MulticastMessage
                {
                    Data = customData,
                    Tokens = firebaseTokens,
                    Notification = new Notification
                    {
                        Title = notificationMessage.Title,
                        Body = notificationMessage.Body,
                    },
                };

                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                notiRepo.SendRealTmeNotification(notificationMessage, customData);
                notiRepo.Save();
                return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });


            }
            if (!IsUrlValid(notificationMessage.ImageUrl))
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationImage(lang) });


            }

            var messa = new MulticastMessage
            {
                Data = customData,
                Tokens = firebaseTokens,
                Notification = new Notification
                {
                    Title = notificationMessage.Title,
                    Body = notificationMessage.Body,
                    ImageUrl = notificationMessage.ImageUrl
                }
            };

            var respo = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(messa);
            notiRepo.SendRealTmeNotification(notificationMessage, customData);
            notiRepo.Save();
            return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });

        }
        #endregion

        #region SendRefundNotification

        [HttpPost("SendRefundNotification")]
        public async Task<IActionResult> SendRefundNotification([FromBody] NotificationMessageFromReund notificationMessage, string lang)
        {

            if (notificationMessage == null)
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationValid(lang) });

            }

            var userIds = notificationMessage.membersId.Select(id => id.ToString()).ToList();

            var firebaseTokens = await dbContext.clientBranchMembers
                .Where(x => userIds.Contains(x.member_id.ToString()) && x.firebase_token != null)
                .Select(x => x.firebase_token)
                .ToListAsync();

            if (firebaseTokens.Count == 0)
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationToken(lang) });

            }
           
            var customData = new Dictionary<string, string>
            {

                { "request_id", notificationMessage.request_id.ToString() },
                { "refund_id", notificationMessage.refund_id.ToString() },
                { "is_refund", notificationMessage.is_refund },

            };

            if (notificationMessage.ImageUrl == string.Empty)
            {
                var message = new MulticastMessage
                {
                    Data = customData,
                    Tokens = firebaseTokens,
                    Notification = new Notification
                    {
                        Title = notificationMessage.Title,
                        Body = notificationMessage.Body,
                    },
                };

                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                notiRepo.AddNotificationFromRefund(notificationMessage, customData);
                notiRepo.Save();
                return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });


            }
            if (!IsUrlValid(notificationMessage.ImageUrl))
            {
                return BadRequest(new MessageDto { Message = Messages.NotificationImage(lang) });


            }

            var messa = new MulticastMessage
            {
                Data = customData,
                Tokens = firebaseTokens,
                Notification = new Notification
                {
                    Title = notificationMessage.Title,
                    Body = notificationMessage.Body,
                    ImageUrl = notificationMessage.ImageUrl
                }
            };

            var respo = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(messa);
            notiRepo.AddNotificationFromRefund(notificationMessage, customData);
            notiRepo.Save();
            return Ok(new MessageDto { Message = Messages.NotificationSend(lang) });

        }
        #endregion

    }



}
