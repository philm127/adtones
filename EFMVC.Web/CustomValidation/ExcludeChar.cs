using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using EFMVC.Data.Repositories;
using EFMVC.Web.ViewModels;

namespace EFMVC.Web.CustomValidation
{
    public class ExcludeCharAttribute : ValidationAttribute
    {
      
        private readonly string _chars;

        public ExcludeCharAttribute(string chars)
            : base("{0} contains invalid character.")
        {
            _chars = chars;
          
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                for (int i = 0; i < _chars.Length; i++)
                {
                    var valueAsString = value.ToString();
                    if (valueAsString.Contains(_chars[i]))
                    {
                        var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                        return new ValidationResult(errorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}