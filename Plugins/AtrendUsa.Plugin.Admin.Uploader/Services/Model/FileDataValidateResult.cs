using System.Collections.Generic;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class FileDataValidateResult<T> : ResultBase
    {
        public FileDataValidateResult(UserContext userContext)
            : base(userContext)
        {
        }

        /// <summary>
        ///     Represents generic collection of File Data(rows)
        /// </summary>
        public List<T> FileData { get; set; }

        public bool ReadyForUpload { get; set; }
    }
}