namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class FileUploadResult : ResultBase
    {
        public FileUploadResult(UserContext userContext) : base(userContext)
        {
        }

        public string TempFilePath { get; set; }

        public string TempFileName { get; set; }
    }
}