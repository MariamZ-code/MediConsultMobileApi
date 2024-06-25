using MailKit.Net.Smtp;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;

using MediConsultMobileApi.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthRepository authRepo;



        private readonly IMemberRepository memberRepo;
        private readonly IValidation validation;

        public LoginController(IAuthRepository authRepo, IMemberRepository memberRepo, IValidation validation)
        {
            this.authRepo = authRepo;

            this.memberRepo = memberRepo;
            this.validation = validation;
        }

        #region GenerateOtp
        private string GenerateOtp()
        {
            // Implement your OTP generation logic (e.g., generate a random 6-digit OTP)
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        #endregion

        #region ValidationRegisteration

        [HttpPost("ValidationRegisteration")]

        public async Task<IActionResult> ValidationRegisteration(ValidatRegisterUserDto userDto, int id, string lang)
        {
            if (ModelState.IsValid)
            {
                var validationResult = await ValidateUser(userDto, id, lang);
                if (validationResult != null)
                {
                    return validationResult;
                }
                #region Send Otp
                string otp = GenerateOtp();
                string text = "Dear Customer , Your OTP is : ";
                string url = $"https://hcms.mediconsulteg.com/sms_api/Message/SendSMS?text={text}{otp}&mobile={userDto.Mobile}";
                using (var client = new HttpClient())
                {
                    var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //GET Method
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var member = authRepo.GetById(id);
                        member.Otp = otp;
                        authRepo.UpdateOtp(member);
                    }
                    else
                    {
                        return BadRequest(new MessageDto { Message = $"{response.StatusCode}" });
                    }
                }
                #endregion


                return Ok(new MessageDto { Message = Messages.DeliveredOtp(lang) });

            }
            return BadRequest(ModelState);
        }

        #endregion


        #region Registeration

        [HttpPost("Registeration")]
        public async Task<IActionResult> Registeration(RegisterUserDto userDto, int id, string lang)
        {

            if (ModelState.IsValid)
            {
                var validateUser = new ValidatRegisterUserDto
                {
                    ConfirmPassword = userDto.ConfirmPassword,
                    Mobile = userDto.Mobile,
                    NationalId = userDto.NationalId,
                    Password = userDto.Password
                };
                var validationResult = await ValidateUser(validateUser, id, lang);
                var member = authRepo.GetById(id);
                if (validationResult != null)
                {
                    return validationResult;
                }
                if (string.IsNullOrEmpty(userDto.Otp))
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterOtp(lang) });

                }
                if (userDto.Otp != member.Otp)
                {
                    return BadRequest(new MessageDto { Message = Messages.IncorrectOtp(lang) });
                }

                authRepo.Registeration(userDto, id);

                authRepo.Save();
                return Ok(new MessageDto { Message = Messages.SuccessRegestration(lang) });

            }
            return BadRequest(ModelState);


        }
        #endregion

        #region ValidateUser
        private async Task<IActionResult> ValidateUser(ValidatRegisterUserDto userDto, int id, string lang)
        {
            if (ModelState.IsValid)
            {
                var memberExists = memberRepo.MemberExists(id);
                var member = authRepo.GetById(id);

                if (!memberExists)
                {

                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                if (member.member_status != "Activated")
                {
                    return BadRequest(new MessageDto { Message = Messages.AccountDisabled(lang) });

                }


                if (member.the_password is not null || !string.IsNullOrEmpty(member.the_password))
                {
                    return BadRequest(new MessageDto { Message = Messages.AccountExists(lang) });
                }
                var memberLoginExists = authRepo.MemberLoginExists(id);
                if (!memberLoginExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                var existingMemberWithSameMobile = memberRepo.GetMemberByMobile(userDto.Mobile);

                var existingMemberWithSameNationalId = memberRepo.GetMemberByNationalId(userDto.NationalId);

                if (string.IsNullOrEmpty(userDto.Mobile))
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterMobile(lang) });

                }
                if (string.IsNullOrEmpty(userDto.NationalId))
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterNationalId(lang) });

                }
                if (string.IsNullOrEmpty(userDto.Password))
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterPassword(lang) });

                }
                if (string.IsNullOrEmpty(userDto.ConfirmPassword))
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterConfirmPassword(lang) });

                }


                if (userDto.Mobile is not null)
                {
                    if (!long.TryParse(userDto.Mobile, out _))
                    {
                        return BadRequest(new MessageDto { Message = Messages.MobileNumber(lang) });


                    }
                    if (!userDto.Mobile.StartsWith("01"))
                    {
                        return BadRequest(new MessageDto { Message = Messages.MobileStartWith(lang) });

                    }
                    if (userDto.Mobile.Length != 11)
                    {
                        return BadRequest(new MessageDto { Message = Messages.MobileNumberFormat(lang) });

                    }

                    //if (existingMemberWithSameMobile != null && existingMemberWithSameMobile.member_id != id)
                    //{
                    //    return BadRequest(new MessageDto { Message = Messages.MobileNumbeExist(lang) });

                    //}


                }

                if (userDto.NationalId is not null)
                {

                    if (!long.TryParse(userDto.NationalId, out _))
                    {
                        return BadRequest(new MessageDto { Message = Messages.NationalIdNumber(lang) });

                    }
                    if (userDto.NationalId.Length != 14)
                    {
                        return BadRequest(new MessageDto { Message = Messages.NationalIdFormat(lang) });


                    }
                    if (existingMemberWithSameNationalId != null && existingMemberWithSameNationalId.member_id != id)
                    {
                        return BadRequest(new MessageDto { Message = Messages.NationalIdExist(lang) });

                    }

                    var (date, gender) = memberRepo.CreateDateAndGender(userDto.NationalId);
                    if (!memberRepo.IsValidDate(date))
                    {
                        return BadRequest(new MessageDto { Message = Messages.NationalIdInvalid(lang) });

                    }
                }
                if (userDto.Password.Length < 8)
                {
                    return BadRequest(new MessageDto { Message = Messages.PasswordFormat(lang) });
                }
                if (userDto.ConfirmPassword != userDto.Password)
                {
                    return BadRequest(new MessageDto { Message = Messages.PasswordAndConfirmPassword(lang) });
                }

                return null;

            }
            return BadRequest(ModelState);
        }
        #endregion




        #region Login

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto userDto, string lang)
        {

            if (string.IsNullOrEmpty(userDto.Id) || string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest(new MessageDto { Message = Messages.PasswordAndIdRequired(lang) });

            }


            if (userDto.Id.Length == 14)
            {
                if (!memberRepo.SSNExists(userDto.Id))
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                var member = memberRepo.GetMemberByNationalId(userDto.Id);
                if (member.member_status != "Activated")
                {
                    return BadRequest(new MessageDto { Message = Messages.AccountDisabled(lang) });
                }
                var user1  = await authRepo.Login(userDto, lang);
                if (user1.Message == Messages.PasswordAndIdIncorrect(lang) || user1.Message == Messages.AccountDisabled(lang))
                {
                    return BadRequest(new MessageDto { Message = Messages.PasswordAndIdIncorrect(lang) });
                }
                return Ok(new MemberIdDto { Id = member.member_id});

            }
            else
            {
                if (!int.TryParse(userDto.Id, out _))
                {
                    return BadRequest(new MessageDto { Message = Messages.InvalidId(lang) });

                }
                var memberExists = memberRepo.MemberExists(int.Parse(userDto.Id));
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }

                var member = memberRepo.MemberDetails(int.Parse(userDto.Id));
                if (member.member_status != "Activated")
                {
                    return BadRequest(new MessageDto { Message = Messages.AccountDisabled(lang) });

                }
                var user = await authRepo.Login(userDto, lang);
                if (user.Message == Messages.AccountDisabled(lang))
                {
                    return BadRequest(new MessageDto { Message = Messages.AccountDisabled(lang) });

                }

                if (user.Message == Messages.PasswordAndIdIncorrect(lang))
                {
                    return BadRequest(new MessageDto { Message = Messages.PasswordAndIdIncorrect(lang) });
                }

                return Ok(user);
            }



        }
        #endregion


        #region ResetPasswordSendOTPSMS

        [HttpGet("ResetPasswordSendOTPSMS")]
        public async Task<IActionResult> ResetPasswordSendOTPSMS(int memberId, string lang)
        {
            if (ModelState.IsValid)
            {
                var member = authRepo.GetById(memberId);
                var memberExists = memberRepo.MemberExists(memberId);
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                var memberMobile = memberRepo.GetByID(memberId).Result.mobile;
                if (member is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });


                }
                if (memberMobile is null || memberMobile == string.Empty)
                {
                    return BadRequest(new MessageDto { Message = Messages.MobileNumberNotFound(lang) });

                }
                if (!memberMobile.StartsWith("01"))
                {
                    return BadRequest(new MessageDto { Message = Messages.MobileStartWith(lang) });

                }
                if (memberMobile.Length != 11)
                {
                    return BadRequest(new MessageDto { Message = Messages.MobileNumberFormat(lang) });

                }

                string otp = GenerateOtp();
                string text = "Dear Customer , Your OTP is : ";


                string url = $"https://hcms.mediconsulteg.com/sms_api/Message/SendSMS?text={text}{otp}&mobile={memberMobile}";
                using (var client = new HttpClient())
                {
                    var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //GET Method
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        authRepo.SendOtp(otp, memberId);

                        return Ok(new MessageDto { Message = Messages.DeliveredOtp(lang) });

                    }
                    else
                    {
                        return BadRequest(response.StatusCode);
                    }
                }

            }
            return BadRequest(ModelState);
        }
        #endregion


        #region ResetPasswordSendOTPEmail
        [HttpGet("ResetPasswordSendOTPEmail")]

        public IActionResult SendEmail(int memberId, string lang)
        {
            var member = authRepo.GetById(memberId);
            var memberExists = memberRepo.MemberExists(memberId);
            if (!memberExists)
            {
                return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

            }
            var memberEmail = memberRepo.GetByID(memberId).Result.email;
            if (member is null)
            {
                return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });


            }

            if (memberEmail is not null)
            {

                if (!validation.IsValidEmail(memberEmail))
                    return BadRequest(new MessageDto { Message = Messages.EmailNotValid(lang) });

            }
            if (memberEmail is null)

                return BadRequest(new MessageDto { Message = Messages.Emailexist(lang) });


            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Mediconsult", "no-reply@mediconsulteg.com"));

            message.To.Add(new MailboxAddress("", memberEmail));

            message.Subject = "Password Reset";

            //message.Headers.Add("X-Priority", "2");
            //message.Headers.Add("X-MSMail-Priority", "Normal");

            string otp = GenerateOtp();

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = otp;

            message.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect("smtp-mail.outlook.com", 587, false);
                client.Authenticate("no-reply@mediconsulteg.com", "Medi@2025");
                client.Send(message);
                client.Disconnect(true);
            }
            authRepo.SendOtp(otp, memberId);

            return Ok(new MessageDto { Message = Messages.DeliveredOtp(lang) });

        }
        #endregion



        #region ChangePassword

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([Required][FromQuery] string otp, ChangePasswordDTO changeDto, [Required] int id, string lang)
        {
            if (ModelState.IsValid)
            {

                var member = authRepo.GetById(id);
                if (member is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });
                }

                if (member.member_id != id)
                {
                    return BadRequest(new MessageDto { Message = Messages.IncorrectId(lang) });

                }
                if (otp != member.Otp)
                {
                    return BadRequest(new MessageDto { Message = Messages.IncorrectOtp(lang) });
                }
                if (changeDto.ConfirmPassword != changeDto.Password)
                {
                    return BadRequest(new MessageDto { Message = Messages.PasswordAndConfirmPassword(lang) });
                }

                authRepo.ChangePass(otp, id, changeDto);

                return Ok(new MessageDto { Message = Messages.ChangePassword(lang) });


            }
            return BadRequest(ModelState);
        }

        #endregion


    }

}
