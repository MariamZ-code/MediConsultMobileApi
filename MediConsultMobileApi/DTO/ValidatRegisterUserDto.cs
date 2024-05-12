namespace MediConsultMobileApi.DTO
{
    public class ValidatRegisterUserDto
    {
        public string Mobile { get; set; }
        public string NationalId { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        //public string Otp { get; set; }
    }
}
