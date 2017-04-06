
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Zit.Utils
{
    public static class ValidateHelper
    {
        public static ValidateResult Validate(this object obj)
        {
            ValidationContext ctx = new ValidationContext(obj, null, null);
            List<ValidationResult> result = new List<ValidationResult>();
            if (Validator.TryValidateObject(obj, ctx, result))
            {
                return new ValidateResult(true, null);
            }
            else
            {
                return new ValidateResult(false, result);
            }
        }
    }

    public class ValidateResult
    {
        internal ValidateResult(bool isValid,IEnumerable<ValidationResult> results)
        {
            IsValid = isValid;
            Results = results;
        }

        public bool IsValid { get; internal set; }

        public IEnumerable<ValidationResult> Results { get; internal set; }

        public string ToMesssage()
        {
            if (!IsValid)
            {
                StringBuilder strB = new StringBuilder();
                foreach (var s in Results)
                {
                    strB.AppendLine(s.ErrorMessage);
                }
                return strB.ToString();
            }
            return null;
        }
    }
}
