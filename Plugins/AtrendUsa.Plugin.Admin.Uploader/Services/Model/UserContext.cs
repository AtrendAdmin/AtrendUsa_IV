using System;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class UserContext
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public override string ToString()
        {
            return string.Format("UserId- {0} ; UserName- {1}", Id, UserName);
        }
    }
}