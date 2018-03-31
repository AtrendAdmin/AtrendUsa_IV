namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public enum UploadStageEnum
    {
        UploadInitiated = 0,
        FileUploadStart = 1,
        FileUploadEnd = 2,
        FileProcessStart = 3,
        FileProcessEnd = 4,
        FileDataValidateStart = 5,
        FileDataValidateEnd = 6,
        FileDataProcessStart = 7,
        FileDataProcessEnd = 8,
        OutputFileProcessStart = 9,
        OutputFileProcessEnd = 10,
        UploadCompleted = 11
    }
}