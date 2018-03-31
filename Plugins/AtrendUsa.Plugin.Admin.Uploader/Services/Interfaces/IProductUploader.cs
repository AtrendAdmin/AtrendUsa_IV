using System.Collections.Generic;
using AtrendUsa.Plugin.Admin.Uploader.Services.Model;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Interfaces
{
    public interface IProductUploader : IUploader<ProductInputRow>
    {
        void Upload(FileUploadInput input);

        UploadStatus GetUploadStatus(string fileTraceToken);

        List<UploadedFileInfo> GetHistory(int take, int skip);

        UploadedFileInfo GetUploadedFileInfoByName(string fileName);
    }
}