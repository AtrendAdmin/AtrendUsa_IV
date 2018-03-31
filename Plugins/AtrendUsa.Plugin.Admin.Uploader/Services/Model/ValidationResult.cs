using System.Collections.Generic;
using System.Linq;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            ValidationErrors = new List<ValidationError>();
        }

        public List<ValidationError> ValidationErrors { get; set; }

        public bool HasViolations
        {
            get { return ValidationErrors.Any(); }
        }
    }
}