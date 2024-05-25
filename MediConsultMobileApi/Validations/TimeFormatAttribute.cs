using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MediConsultMobileApi.Validations
{
    public class TimeFormatAttribute : ValidationAttribute
    {
        private readonly string _timeFormat;

        public TimeFormatAttribute(string timeFormat)
        {
            _timeFormat = timeFormat;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (DateTime.TryParseExact(value.ToString(), _timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult($"The field {validationContext.DisplayName} must be in the format '{_timeFormat}'.");
            }
            return ValidationResult.Success;
        }
    }

}
