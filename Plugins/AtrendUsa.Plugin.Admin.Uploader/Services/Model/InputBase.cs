using System;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class InputBase
    {
        public InputBase(UserContext userContext)
        {
            if (userContext == null) throw new ArgumentNullException("userContext");
            UserContext = userContext;
        }

        public UserContext UserContext { get; set; }
    }
}