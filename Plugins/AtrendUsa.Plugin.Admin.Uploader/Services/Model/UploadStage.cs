namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class UploadStage
    {
        public UploadStageEnum Stage { get; set; }

        public string Message { get; set; }

        public object StageOutput { get; set; }
    }
}