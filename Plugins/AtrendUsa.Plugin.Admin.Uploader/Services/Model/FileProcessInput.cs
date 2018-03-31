namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class FileProcessInput : InputBase
    {
        public FileProcessInput(UserContext userContext) : base(userContext)
        {
        }

        public string FileName { get; set; }

        public string FilePath { get; set; }
    }
}