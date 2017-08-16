using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace wedding.Models
{
    public class FutureDateAttribute : ValidationAttribute, IClientModelValidator
    {
        private DateTime _today;

        public void AddValidation(ClientModelValidationContext context)
        {
            // throw new NotImplementedException();
        }

        public FutureDateAttribute()
        {
            _today = DateTime.Today;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Wedding wedding = (Wedding)validationContext.ObjectInstance;

            if (wedding.WeddingDate < _today)
            {
                return new ValidationResult("Date must be in the future.");
            }

            return ValidationResult.Success;
        }
    }
}