using AtrendUsa.Plugin.Admin.Uploader.Services.Model;
using NPOI.XSSF.UserModel;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Interfaces
{
    public interface IProductValidator
    {
        ValidationResult InputFileValidate(InputFile input);

        ValidationResult ExcelFileValidate(XSSFWorkbook uploadedExcel);

        ValidationResult FileFormatValidate(string fileFormat);
    }
}