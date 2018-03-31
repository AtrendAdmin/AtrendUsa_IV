using AtrendUsa.Plugin.Admin.Uploader.Services.Model;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Interfaces
{
    /// <summary>
    ///     Provides base interface for any upload process
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUploader<T>
    {
        FileUploadResult FileUpload(FileUploadInput input);

        FileProcessResult<T> FileProcess(FileProcessInput input);

        FileDataValidateResult<T> FileDataValidate(FileDataValidateInput<T> input);

        FileDataProcessResult<T> FileDataProcess(FileDataProcessInput<T> input);

        OutputFileProcessResult OutputFileProcess(OutputFileProcessInput<T> input);
    }
}