using System.ComponentModel.DataAnnotations;

namespace LojiteksEntity.DataAnnotaionAttribute
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        // Set the name of the property to compare
        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        // Validate the date comparison
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateTime)value;

            var comparisonValue = (DateTime)validationContext.ObjectType.GetProperty(_comparisonProperty)
                                                                        .GetValue(validationContext.ObjectInstance);

            if (currentValue < comparisonValue)
            {
                return new ValidationResult(ErrorMessage = "End date must be later than start date");
            }

            return ValidationResult.Success;
        }
    }
}
