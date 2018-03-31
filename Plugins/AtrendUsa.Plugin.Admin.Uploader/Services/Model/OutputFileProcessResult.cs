namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class OutputFileProcessResult : ResultBase
    {
        public OutputFileProcessResult(UserContext userContext) : base(userContext)
        {
        }

        public UploadedFileInfo OutputFileInfo { get; set; }
    }
}