using System.ComponentModel.DataAnnotations;

namespace AOMForum.Web.Infrastructure.Attributes
{
    public class MinAgeRestrictionAttribute : ValidationAttribute
    {
        private readonly int minAgeValue;

        public MinAgeRestrictionAttribute(int minAgeValue) => this.minAgeValue = minAgeValue;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateTime? dateOfBirth = (DateTime?)value;
            if (!dateOfBirth.HasValue)
            {
                return null;
            }

            int currentYear = DateTime.UtcNow.Year;
            int yearOfBirth = dateOfBirth.Value.Year;
            int age = currentYear - yearOfBirth;
            if (dateOfBirth > DateTime.UtcNow.AddYears(-age))
            {
                age--;
            }

            if (age < this.minAgeValue)
            {
                return new ValidationResult(string.Format(this.ErrorMessage, this.minAgeValue));
            }

            return ValidationResult.Success;
        }
    }
}