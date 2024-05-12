using System.Collections.Concurrent;
using System.Net.Mail;
using System.Net;

namespace MediConsultMobileApi.Repository
{
    public class PasswordReset
    {
        #region ChangePassword

            private static readonly ConcurrentDictionary<string, string> UserOtpDictionary = new ConcurrentDictionary<string, string>();

            public static bool RequestPasswordReset(string identifier, string userEmail)
            {
                // Check if the user exists and get their email address

                // Generate OTP
                string otp = GenerateOtp();

                // Store OTP associated with the user
                UserOtpDictionary.TryAdd(identifier, otp);

                // Send OTP via email
                SendOtpEmail(userEmail, otp);

                return true; // Password reset request successful
            }

            public static bool VerifyOtp(string identifier, string otp)
            {
                // Verify OTP against the stored OTP
                if (UserOtpDictionary.TryGetValue(identifier, out var storedOtp))
                {
                    return string.Equals(otp, storedOtp, StringComparison.OrdinalIgnoreCase);
                }

                return false; // OTP verification failed
            }

            private static string GenerateOtp()
            {
                // Generate a random 6-digit OTP (for example)
                Random random = new Random();
                return random.Next(100000, 999999).ToString();
            }

            private static void SendOtpEmail(string toEmail, string otp)
            {
                // Configure and send the email
                using (var client = new SmtpClient("your-smtp-server.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("your-email@example.com", "your-email-password");
                    client.EnableSsl = true;

                    var mail = new MailMessage("your-email@example.com", toEmail)
                    {
                        Subject = "Password Reset OTP",
                        Body = $"Your OTP for password reset is: {otp}"
                    };

                    client.Send(mail);
                }
            }
        }

        #endregion
    }

