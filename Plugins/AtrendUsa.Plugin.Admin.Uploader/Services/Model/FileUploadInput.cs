using System.IO;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class FileUploadInput : InputBase
    {
        public FileUploadInput(UserContext userContext) : base(userContext)
        {
        }

        public string UploadTraceToken { get; set; }

        public string FileName { get; set; }

        public long ContentLength { get; set; }

        public string ContentType { get; set; }

        public Stream InputStream { get; set; }
    }
}