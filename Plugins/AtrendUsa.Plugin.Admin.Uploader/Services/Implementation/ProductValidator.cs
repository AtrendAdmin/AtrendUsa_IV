using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AtrendUsa.Plugin.Admin.Uploader.Services.Extensions;
using AtrendUsa.Plugin.Admin.Uploader.Services.Interfaces;
using AtrendUsa.Plugin.Admin.Uploader.Services.Model;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Implementation
{
    public class ProductValidator : IProductValidator
    {
        #region private props

        enum CellPosition
        {
            First,
            Last
        }

        private readonly Dictionary<string, CellPosition> _headerValues = new Dictionary<string, CellPosition>
        {
            //first column
            {"Sku", CellPosition.First},
            //last column
            {"UploadedProductId", CellPosition.Last}
        };

        private readonly int _maxBatchSize = int.Parse(ConfigurationManager.AppSettings["MaxBatchSize"]);

        #endregion

        #region IProductValidator Members

        public ValidationResult InputFileValidate(InputFile input)
        {
            if (input == null) throw new ArgumentNullException("input");
            var result = new ValidationResult();

            //validate header
            if (input.Header == null || !input.Header.Any())
            {
                result.ValidationErrors.Add(new ValidationError("Header",
                    "Header is missing. Please use approved template!"));
                return result;
            }

            IRow descritpionRow = input.Header.Last();
            foreach (var headerValue in _headerValues)
            {
                ICell cell = descritpionRow.GetCell(headerValue.Value == CellPosition.First ? descritpionRow.FirstCellNum : (descritpionRow.LastCellNum - 1), MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellType(CellType.String);
                if (!headerValue.Key.InvariantEquals(cell.StringCellValue.Trim()))
                {
                    result.ValidationErrors.Add(new ValidationError("Header",
                        "Invalid file structure. Please use approved template!"));
                    return result;
                }
            }

            // validate min allowed
            if (input.Data == null || !input.Data.Any())
            {
                result.ValidationErrors.Add(new ValidationError("Data",
                    "No records provided. At least one Product required to initiate upload!"));
                return result;
            }

            //Validate max allowed
            int maxWarehouseRows = _maxBatchSize;
            int actualWarehouseRows = input.Data.Count;
            if (actualWarehouseRows > maxWarehouseRows)
            {
                result.ValidationErrors.Add(new ValidationError("MaxRows",
                    string.Format(
                        "Batch size has been exceeded. Maximum allowed size is {0} rows, file provided has: {1} rows",
                        maxWarehouseRows, actualWarehouseRows)));
                return result;
            }

            return result;
        }

        public ValidationResult ExcelFileValidate(XSSFWorkbook input)
        {
            //if (input == null) throw new ArgumentNullException("input");
            //var result = new ValidationResult();
            //// Validate that file has schema and root element has appropriate name
            //var xmlMappings = input.GetCustomXMLMappings();
            //if (!xmlMappings.Any() || xmlMappings.First().GetCTMap().RootElement != RootElementName)
            //{
            //    result.ValidationErrors.Add(new ValidationError("Format", "Invalid file structure. Please use approved template!"));
            //    return result;
            //}
            //return result;

            //todo: fix this
            return new ValidationResult();
        }

        public ValidationResult FileFormatValidate(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            var result = new ValidationResult();
            if (!input.Contains("vnd.openxmlformats-officedocument.spreadsheetml.sheet") &&
                !input.Contains("application/octet-stream"))
            {
                result.ValidationErrors.Add(new ValidationError("Format", input + " format is not supported!"));
            }
            return result;
        }

        #endregion
    }
}