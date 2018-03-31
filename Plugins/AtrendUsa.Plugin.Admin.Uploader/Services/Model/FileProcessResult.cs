using System.Collections.Generic;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class FileProcessResult<T> : ResultBase
    {
        public FileProcessResult(UserContext userContext) : base(userContext)
        {
        }

        /// <summary>
        ///     Represents generic collection of File Data(rows)
        /// </summary>
        public List<T> FileData { get; set; }

        public int HeadersCount { get; set; }

        // Add User?
    }
}