using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using AtrendUsa.Plugin.Admin.Uploader.Services.Extensions;
using AtrendUsa.Plugin.Admin.Uploader.Services.Helpers;
using AtrendUsa.Plugin.Admin.Uploader.Services.Interfaces;
using AtrendUsa.Plugin.Admin.Uploader.Services.Model;
using AtrendUsa.Plugin.Admin.Uploader.Services.Utils;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Services.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Implementation
{
    public class ProductUploader : IProductUploader
    {
        private static readonly string FileSharePath = ConfigurationManager.AppSettings["FileShare"];
        private static readonly string FileShareTempFilesPath = Path.Combine(FileSharePath, "Temp");
        private static readonly string FileShareProcessedFilesPath = Path.Combine(FileSharePath, "Processed");
        private readonly ICacheManager _cacher;

        private readonly ILogger _logger;
        private readonly IProductResolver _productResolver;
        private readonly IProductValidator _validator;

        public ProductUploader(ILogger logger, IProductValidator validator, ICacheManager cacher,
            IProductResolver productResolver)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            if (validator == null) throw new ArgumentNullException("validator");
            if (cacher == null) throw new ArgumentNullException("cacher");
            if (productResolver == null) throw new ArgumentNullException("productResolver");
            _logger = logger;
            _validator = validator;
            _cacher = cacher;
            _productResolver = productResolver;
        }

        #region IProductUploader Members

        /// <summary>
        ///     Ensures the process chain of uploading Product info: file upload,process, validation, mapping, address validation,
        ///     warehouse creation, output file generation
        ///     Updates upload status after each step completes
        /// </summary>
        /// <param name="input"></param>
        public void Upload(FileUploadInput input)
        {
            if (input == null) throw new ArgumentNullException("input");

            var uploadStatus = new UploadStatus(input.UploadTraceToken);
            _logger.Information(string.Format("Upload initiated for file: {0}, by {1}", input.FileName,
                input.UserContext));
            try
            {
                //Initiate upload
                UpdateStatus(uploadStatus, UploadStageEnum.UploadInitiated, "Initiating upload process...");
                //Upload file to temp directory
                UpdateStatus(uploadStatus, UploadStageEnum.FileUploadStart, "File upload to temp directory started...");
                FileUploadResult fileUploadResult = FileUpload(input);
                UpdateStatus(uploadStatus, UploadStageEnum.FileUploadEnd, "File upload to temp directory completed.");
                if (fileUploadResult.ValidationResult.HasViolations)
                {
                    UpdateStatus(uploadStatus, UploadStageEnum.UploadCompleted,
                        "Unable to complete upload process. " +
                        fileUploadResult.ValidationResult.ValidationErrors.First().Message);
                    return;
                }
                // Load File Data into memory
                UpdateStatus(uploadStatus, UploadStageEnum.FileProcessStart, "Load file data into memory started...");
                FileProcessResult<ProductInputRow> fileProcessResult =
                    FileProcess(new FileProcessInput(input.UserContext)
                    {
                        FileName = fileUploadResult.TempFileName,
                        FilePath = fileUploadResult.TempFilePath
                    });
                UpdateStatus(uploadStatus, UploadStageEnum.FileProcessEnd, "Load file data into memory completed.");
                if (fileProcessResult.ValidationResult.HasViolations)
                {
                    UpdateStatus(uploadStatus, UploadStageEnum.UploadCompleted,
                        "Unable to complete upload process. " +
                        fileProcessResult.ValidationResult.ValidationErrors.First().Message);
                    return;
                }
                // Validate data
                UpdateStatus(uploadStatus, UploadStageEnum.FileDataValidateStart, "File  data validation started...");
                FileDataValidateResult<ProductInputRow> fileDataValidateResult =
                    FileDataValidate(new FileDataValidateInput<ProductInputRow>(input.UserContext)
                    {
                        FileData = fileProcessResult.FileData
                    });
                FileDataProcessResult<ProductInputRow> fileDataProcessResult = null;
                if (fileDataValidateResult.ValidationResult.HasViolations)
                {
                    UpdateStatus(uploadStatus, UploadStageEnum.FileDataValidateEnd,
                        "File data validation completed. " +
                        fileDataValidateResult.ValidationResult.ValidationErrors.First().Message);
                }
                else
                {
                    UpdateStatus(uploadStatus, UploadStageEnum.FileDataValidateEnd, "File data validation completed.");
                }
                //process data if ready
                if (fileDataValidateResult.ReadyForUpload)
                {
                    UpdateStatus(uploadStatus, UploadStageEnum.FileDataProcessStart, "File data processing started...");
                    fileDataProcessResult =
                        FileDataProcess(new FileDataProcessInput<ProductInputRow>(input.UserContext)
                        {
                            FileData = fileDataValidateResult.FileData
                        });
                    if (fileDataProcessResult.ValidationResult.HasViolations)
                    {
                        UpdateStatus(uploadStatus, UploadStageEnum.FileDataProcessEnd,
                            "File data processing completed. " +
                            fileDataProcessResult.ValidationResult.ValidationErrors.First().Message);
                    }
                    else
                    {
                        UpdateStatus(uploadStatus, UploadStageEnum.FileDataProcessEnd, "File data processing completed.");
                    }
                }

                UpdateStatus(uploadStatus, UploadStageEnum.OutputFileProcessStart, "Output file generation started...");
                OutputFileProcessResult outputFileProcessResult =
                    OutputFileProcess(new OutputFileProcessInput<ProductInputRow>(input.UserContext)
                    {
                        TempFileName = fileUploadResult.TempFileName,
                        TempFilePath = fileUploadResult.TempFilePath,
                        ProcessedFileData =
                            fileDataProcessResult == null
                                ? fileDataValidateResult.FileData
                                : fileDataProcessResult.FileData,
                        HeadersCount = fileProcessResult.HeadersCount
                    });
                UpdateStatus(uploadStatus, UploadStageEnum.OutputFileProcessEnd, "Output file generation completed.");
                UpdateStatus(uploadStatus, UploadStageEnum.UploadCompleted, "Upload process completed.",
                    outputFileProcessResult.OutputFileInfo);
            }
            catch (Exception e)
            {
                UpdateStatus(uploadStatus, UploadStageEnum.UploadCompleted,
                    "Unable to complete upload process. An unexpected error has occurred.");
                _logger.Fatal("Unable to complete upload process. User context: " + input.UserContext, e);
            }
        }


        /// <summary>
        ///     Retreive upload status
        /// </summary>
        /// <returns></returns>
        public UploadStatus GetUploadStatus(string fileTraceToken)
        {
            if (string.IsNullOrWhiteSpace(fileTraceToken)) throw new ArgumentNullException("fileTraceToken");
            return _cacher.Get(fileTraceToken, () => new UploadStatus());
        }

        public List<UploadedFileInfo> GetHistory(int take, int skip)
        {
            if (Directory.Exists(FileShareProcessedFilesPath))
            {
                List<FileInfo> processedFiles = new DirectoryInfo(FileShareProcessedFilesPath)
                    .GetFiles("*.*", SearchOption.AllDirectories)
                    .OrderByDescending(x => x.CreationTime)
                    .Skip(skip).Take(take)
                    .ToList();
                return
                    processedFiles.ConvertAll(
                        x =>
                            new UploadedFileInfo
                            {
                                FileName = x.Name,
                                FilePath = x.FullName,
                                UploadDate = x.CreationTime.ToString("dd-MM-yy hh:mm")
                            });
            }
            return new List<UploadedFileInfo>();
        }

        public UploadedFileInfo GetUploadedFileInfoByName(string fileName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Upload Excel File to temp directory on the server for further processing
        ///     Validates format and structure of the file. Saves temp file to tem directory.
        /// </summary>
        /// <param name="input">FileUploadInput</param>
        /// <returns>FileUploadResult</returns>
        public FileUploadResult FileUpload(FileUploadInput input)
        {
            if (input == null) throw new ArgumentNullException("input");
            _logger.Information(string.Format("Uploading to temp directory. File name:  {0}", input.FileName));
            using (input.InputStream)
            {
                var result = new FileUploadResult(input.UserContext);

                //validate input format
                ValidationResult formatValidationResult = _validator.FileFormatValidate(input.ContentType);
                if (formatValidationResult.HasViolations)
                {
                    result.ValidationResult.ValidationErrors = formatValidationResult.ValidationErrors;
                    return result;
                }

                var uploadedExcel = new XSSFWorkbook(input.InputStream);

                //validate excel file
                ValidationResult excelFileValidationResult = _validator.ExcelFileValidate(uploadedExcel);
                if (excelFileValidationResult.HasViolations)
                {
                    result.ValidationResult.ValidationErrors = excelFileValidationResult.ValidationErrors;
                    return result;
                }

                string tempFileName = Utility.ToUniqueFileName(input.FileName);
                string tempDirectoryPath = FileShareTempFilesPath;
                string tempFilePath = Utility.GetFilePath(tempDirectoryPath, tempFileName);
                using (var fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    uploadedExcel.Write(fs);
                    result.TempFileName = tempFileName;
                    result.TempFilePath = tempFilePath;
                }
                return result;
            }
        }

        /// <summary>
        ///     Processing of file(temp) that has been uploaded to the server.
        ///     Converts file to input data for further processing
        /// </summary>
        /// <param name="input">FileProcessResult</param>
        /// <returns>FileProcessInput</returns>
        public FileProcessResult<ProductInputRow> FileProcess(FileProcessInput input)
        {
            if (input == null) throw new ArgumentNullException("input");
            var result = new FileProcessResult<ProductInputRow>(input.UserContext);
            _logger.Information(string.Format("Processing temp file. File path:  {0}", input.FilePath));

            //convert 
            InputFile inputFile = LoadInputFile(input.FilePath);

            ValidationResult inputDataValidateResult = _validator.InputFileValidate(inputFile);
            if (inputDataValidateResult.HasViolations)
            {
                result.ValidationResult.ValidationErrors = inputDataValidateResult.ValidationErrors;
                //remove temp file if failed
                File.Delete(input.FilePath);

                return result;
            }
            result.FileData = inputFile.Data;
            result.HeadersCount = inputFile.Header.Last().LastCellNum;
            return result;
        }

        public FileDataValidateResult<ProductInputRow> FileDataValidate(FileDataValidateInput<ProductInputRow> input)
        {
            if (input == null) throw new ArgumentNullException("input");
            var result = new FileDataValidateResult<ProductInputRow>(input.UserContext) {ReadyForUpload = true};
            int totalCount = input.FileData.Count;
            int failedCount = 0;
            _logger.Information(string.Format("Validating file data. Total count: {0}", totalCount));

            //validate data
            foreach (ProductInputRow row in input.FileData)
            {
                row.Validated = true; //todo fix this, actuallyadd validation
                //if (DataQualityValidate(row, input.UserContext, ref failedCount))
                //{
                //    AddressValidate(row, input.UserContext, ref failedCount);
                //}
            }

            if (failedCount != 0)
            {
                if (failedCount == totalCount)
                {
                    result.ReadyForUpload = false;
                    result.ValidationResult.ValidationErrors.Add(new ValidationError("Data",
                        string.Format("All {0} records failed validation. Please check output file for details",
                            failedCount)));
                }
                else
                {
                    result.ValidationResult.ValidationErrors.Add(new ValidationError("Data",
                        string.Format("{0} out of {1} records failed validation. Please check output file for details",
                            failedCount, totalCount)));
                }
            }

            result.FileData = input.FileData;
            return result;
        }

        /// <summary>
        ///     Process file data row by row
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FileDataProcessResult<ProductInputRow> FileDataProcess(FileDataProcessInput<ProductInputRow> input)
        {
            if (input == null) throw new ArgumentNullException("input");
            var result = new FileDataProcessResult<ProductInputRow>(input.UserContext);
            int totalCount = input.FileData.Count;
            int failedCount = 0;
            _logger.Information(string.Format("Processing file data. Total count: {0}", totalCount));
            //process data
            foreach (ProductInputRow row in input.FileData.Where(x => x.Validated))
            {
                ProcessInputRow(row, input.UserContext, ref failedCount);
            }

            if (failedCount != 0)
            {
                if (failedCount == totalCount)
                {
                    result.ValidationResult.ValidationErrors.Add(new ValidationError("Data",
                        string.Format("All {0} records failed to be processed. Please check output file for details",
                            failedCount)));
                }
                else
                {
                    result.ValidationResult.ValidationErrors.Add(new ValidationError("Data",
                        string.Format(
                            "{0} out of {1} records failed to be processed. Please check output file for details",
                            failedCount, totalCount)));
                }
            }

            result.FileData = input.FileData;
            return result;
        }

        public OutputFileProcessResult OutputFileProcess(OutputFileProcessInput<ProductInputRow> input)
        {
            if (input == null) throw new ArgumentNullException("input");

            //todo: think about this
            int uploadStatusCellIndex = input.HeadersCount - 3;
            int uploadMessageCellIndex = input.HeadersCount - 2;
            int uploadedProductIdCellIndex = input.HeadersCount - 1;

            var result = new OutputFileProcessResult(input.UserContext);

            string newFileName = input.UserContext.Id + "_" + input.TempFileName;
            string newDirectoryPath = Path.Combine(FileShareProcessedFilesPath, input.UserContext.Id.ToString());
            string newFilePath = Utility.GetFilePath(newDirectoryPath, newFileName);

            _logger.Information(string.Format("Generating output file. File path: {0}", newFilePath));

            using (var fs = new FileStream(input.TempFilePath, FileMode.Open, FileAccess.Read))
            {
                var uploadedExcel = new XSSFWorkbook(fs);
                //get data sheet
                ISheet dataSheet = uploadedExcel.GetSheetAt(0);
                //update file with processed info
                foreach (ProductInputRow productInputRow in input.ProcessedFileData)
                {
                    IRow row = dataSheet.GetRow(productInputRow.RowIndex);
                    row.GetCell(uploadedProductIdCellIndex, MissingCellPolicy.CREATE_NULL_AS_BLANK)
                        .SetCellValue(productInputRow.UploadedProductId);
                    row.GetCell(uploadStatusCellIndex, MissingCellPolicy.CREATE_NULL_AS_BLANK)
                        .SetCellValue(productInputRow.UploadStatus);
                    row.GetCell(uploadMessageCellIndex, MissingCellPolicy.CREATE_NULL_AS_BLANK)
                        .SetCellValue(productInputRow.UploadMessage);
                }
                //write back to file
                using (var ws = new FileStream(input.TempFilePath, FileMode.Create, FileAccess.Write))
                {
                    uploadedExcel.Write(ws);
                }
            }
            //move and rename file
            File.Move(input.TempFilePath, newFilePath);

            result.OutputFileInfo = new UploadedFileInfo
            {
                FileName = newFileName,
                FilePath = newFilePath,
                UploadDate = DateTime.Now.ToString("dd-MM-yy hh:mm"),
            };

            return result;
        }

        #endregion

        #region private methods

        private void UpdateStatus(UploadStatus uploadStatus, UploadStageEnum step, string message,
            object stageOutput = null)
        {
            uploadStatus.CurrentUploadStage = new UploadStage
            {
                Stage = step,
                Message = string.Format("{0} - {1}", DateTime.Now.ToString("hh:mm:ss"), message),
                StageOutput = stageOutput
            };
            _cacher.Set(uploadStatus.TraceToken, uploadStatus, 60);
            _logger.Information(message);
        }

        private void ProcessInputRow(ProductInputRow inputRow, UserContext userContext, ref int failedCount)
        {
            //todo add debug log
            inputRow.Processed = true;
            try
            {
                Product resolvedProduct =
                    _productResolver.Resolve(new ProductResolveInput(userContext) {ProductInputRow = inputRow});
                inputRow.UploadedProductId = resolvedProduct.Id.ToString();
                inputRow.UploadStatus = "Data processing completed";
                inputRow.UploadMessage = string.Empty;
            }
            catch (Exception e)
            {
                _logger.Error("Unable to process input row. " + userContext, e);
                inputRow.UploadStatus = "Failed to process data";
                inputRow.UploadMessage = "Unable to process input row.";
                failedCount++;
            }
        }

        private static InputFile LoadInputFile(string filePath)
        {
            var fileRowData = new Dictionary<int, IRow>();

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int index = 0;
                var uploadedExcel = new XSSFWorkbook(fs);
                //get data sheet
                ISheet dataSheet = uploadedExcel.GetSheetAt(0);
                // preload into memory and close the stream
                IEnumerator rowEnumerator = dataSheet.GetEnumerator();
                while (rowEnumerator.MoveNext())
                {
                    fileRowData.Add(index, (IRow) rowEnumerator.Current);
                    index++;
                }
            }
            //take header rows
            List<IRow> header = fileRowData.Take(1).ToList().ConvertAll(x => x.Value);
            //skip header rows
            List<ProductInputRow> productInputRowData =
                fileRowData.Skip(1).ToList().ConvertAll(x => Converter.Convert(x.Value, x.Key, header.Last()))
                    .Where(x => !x.AllStringPropertiesNullOrEmpty())
                    // filter out blanked out data: NPOI reads them as rows
                    .ToList();
            return new InputFile {Header = header, Data = productInputRowData};
        }

        #endregion
    }
}