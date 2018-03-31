using System;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public abstract class ResultBase
    {
        public ResultBase(UserContext userContext)
        {
            if (userContext == null) throw new ArgumentNullException("userContext");
            ValidationResult = new ValidationResult();
            UserContext = userContext;
        }

        public ValidationResult ValidationResult { get; set; }

        public UserContext UserContext { get; set; }
    }
}