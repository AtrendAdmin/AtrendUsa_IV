using System.Collections.Generic;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class OutputFileProcessInput<T> : InputBase
    {
        public OutputFileProcessInput(UserContext userContext) : base(userContext)
        {
        }

        public string TempFileName { get; set; }

        public string TempFilePath { get; set; }

        /// <summary>
        ///     Represents generic collection of File Data(rows)
        /// </summary>
        public List<T> ProcessedFileData { get; set; }

        public int HeadersCount { get; set; }
    }
}