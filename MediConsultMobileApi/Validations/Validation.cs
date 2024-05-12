using System.Text.RegularExpressions;

namespace MediConsultMobileApi.Validations
{
    public class Validation : IValidation
    {
        public bool IsValidEmail(string email)
        {
          //if (email == null) { return true; }
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

          
            Regex regex = new Regex(pattern);
            Match match = regex.Match(email);
            return match.Success;
        }
    }
}
