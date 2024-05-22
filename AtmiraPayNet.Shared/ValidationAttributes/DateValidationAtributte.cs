using System.ComponentModel.DataAnnotations;

namespace AtmiraPayNet.Shared.ValidationAttributes
{
    internal class DateValidationAtributte : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var today = DateTime.Now;

            var dateOfBirth = (DateOnly?)value;

            if (dateOfBirth != null)
            {
                if (today.Year - dateOfBirth.Value.Year < 18)
                {
                    return new ValidationResult("El usuario debe ser mayor de edad");
                }
                else if (today.Year - dateOfBirth.Value.Year == 18)
                {
                    if (today.Month < dateOfBirth.Value.Month)
                    {
                        return new ValidationResult("El usuario debe ser mayor de edad");
                    }
                    else if (today.Month == dateOfBirth.Value.Month)
                    {
                        if (today.Day < dateOfBirth.Value.Day)
                        {
                            return new ValidationResult("El usuario debe ser mayor de edad");
                        }
                    }
                }

                if (today.Year - dateOfBirth.Value.Year > 80)
                {
                    return new ValidationResult("El usuario no puede tener más de 80 años");
                }
                else if (today.Year - dateOfBirth.Value.Year == 80)
                {
                    if (today.Month > dateOfBirth.Value.Month)
                    {
                        return new ValidationResult("El usuario no puede tener más de 80 años");
                    }
                    else if (today.Month == dateOfBirth.Value.Month)
                    {
                        if (today.Day > dateOfBirth.Value.Day)
                        {
                            return new ValidationResult("El usuario no puede tener más de 80 años");
                        }
                    }
                }
            }

            return ValidationResult.Success!;
        }
    }
}
