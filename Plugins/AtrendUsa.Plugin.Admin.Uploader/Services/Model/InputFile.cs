using System.Collections.Generic;
using NPOI.SS.UserModel;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class InputFile
    {
        public List<IRow> Header { get; set; }

        public List<ProductInputRow> Data { get; set; }
    }
}