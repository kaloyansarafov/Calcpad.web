using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Calcpad.web.Validation
{
    public class ValidNumbers : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string[] fields)
            {
                List<string> errors = null;
                for (int i = 0, len = fields.Length; i < len; ++i)
                {
                    var s = fields[i].AsSpan();
                    if (s.Length > 0)
                    {
                        var j = s.IndexOf(':');
                        if (j > 0)
                            s = s[(j + 1)..];
                    }
                    if (s.Length == 0 || s[0] == '+' || !double.TryParse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var _))
                    {
                        errors ??= new();
                        errors.Add(fields[i]);
                    }
                        
                }
                if (errors is not null) 
                {
                    if (errors.Count == 1)
                        return new ValidationResult($"Invalid number: '{errors[0]}'.");

                    return new ValidationResult($"Invalid numbers: '{string.Join("', '", errors)}'.");
                }
            }
            else
                return new ValidationResult($"{validationContext.DisplayName} must be array of strings.");

            return ValidationResult.Success;
        }
    }
}
