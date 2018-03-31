using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using AtrendUsa.Plugin.Admin.Uploader.Services.Extensions;
using AtrendUsa.Plugin.Admin.Uploader.Services.Interfaces;
using AtrendUsa.Plugin.Admin.Uploader.Services.Model;
using Autofac;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using NUnit.Framework;

namespace AtrendUsa.Plugins.IntegrationTests.Admin.Uploader
{
    /// <summary>
    ///     Tests against IUploader implementation
    /// </summary>
    [TestFixture] //todo: use factory, refactor dry
    public class ProductUploaderTests
    {
        private ContainerManager _containerManager;

        [SetUp]
        public void TestSetup()
        {
        }

        [TearDown]
        public void TestTearDown()
        {
        }

        private static string _fileSharePath;
        private static Guid _createAsUser;
        private static UserContext _userContext;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _fileSharePath = ConfigurationManager.AppSettings["FileShare"];
            _createAsUser = new Guid(ConfigurationManager.AppSettings["CreateAsUser"]);
            _userContext = new UserContext {Id = _createAsUser, UserName = "vladfv@gmail.com"};
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
        }

        #region IUploader tests

        [Test]
        public void FileProcess_InvalidFileWithExtraColumns_ShouldReturnInvalidFormat()
        {
            const string fileName = "AtrendProductTemplate_ExtraColumns.xlsx";
            string testfilePath = Path.Combine(_fileSharePath, "Test Files", fileName);
            var productUploader = EngineContext.Current.Resolve<IProductUploader>();
            var stream = new FileStream(testfilePath, FileMode.Open, FileAccess.Read);
            var input = new FileUploadInput(_userContext)
            {
                ContentLength = stream.Length,
                ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = fileName,
                InputStream = stream
            };
            FileUploadResult uploadResult = productUploader.FileUpload(input);
            FileProcessResult<ProductInputRow> processResult =
                productUploader.FileProcess(new FileProcessInput(_userContext)
                {
                    FileName = uploadResult.TempFileName,
                    FilePath = uploadResult.TempFilePath
                });
            ValidationError validationError = processResult.ValidationResult.ValidationErrors.FirstOrDefault() ??
                                              new ValidationError();

            Assert.AreEqual(true, processResult.ValidationResult.HasViolations, validationError.Message);
            Assert.AreEqual("Invalid file structure. Please use approved template!", validationError.Message);
        }

        [Test]
        public void FileProcess_InvalidFileWithMissingColumns_ShouldReturnInvalidFormat()
        {
            const string fileName = "AtrendProductTemplate_MissingColumns.xlsx";
            string testfilePath = Path.Combine(_fileSharePath, "Test Files", fileName);
            var productUploader = EngineContext.Current.Resolve<IProductUploader>();
            var stream = new FileStream(testfilePath, FileMode.Open, FileAccess.Read);
            var input = new FileUploadInput(_userContext)
            {
                ContentLength = stream.Length,
                ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = fileName,
                InputStream = stream
            };
            FileUploadResult uploadResult = productUploader.FileUpload(input);
            FileProcessResult<ProductInputRow> processResult =
                productUploader.FileProcess(new FileProcessInput(_userContext)
                {
                    FileName = uploadResult.TempFileName,
                    FilePath = uploadResult.TempFilePath
                });
            ValidationError validationError = processResult.ValidationResult.ValidationErrors.FirstOrDefault() ??
                                              new ValidationError();

            Assert.AreEqual(true, processResult.ValidationResult.HasViolations, validationError.Message);
            Assert.AreEqual("Invalid file structure. Please use approved template!", validationError.Message);
        }

        [Test]
        public void FileProcess_InvalidFileWithMissingHeader_ShouldReturnInvalidFormat()
        {
            const string FileName = "AtrendProductTemplate_MissingHeader.xlsx";
            string testfilePath = Path.Combine(_fileSharePath, "Test Files", FileName);
            var ProductUploader = EngineContext.Current.Resolve<IProductUploader>();
            var stream = new FileStream(testfilePath, FileMode.Open, FileAccess.Read);
            var input = new FileUploadInput(_userContext)
            {
                ContentLength = stream.Length,
                ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = FileName,
                InputStream = stream
            };
            FileUploadResult uploadResult = ProductUploader.FileUpload(input);
            FileProcessResult<ProductInputRow> processResult =
                ProductUploader.FileProcess(new FileProcessInput(_userContext)
                {
                    FileName = uploadResult.TempFileName,
                    FilePath = uploadResult.TempFilePath
                });
            ValidationError validationError = processResult.ValidationResult.ValidationErrors.FirstOrDefault() ??
                                              new ValidationError();

            Assert.AreEqual(true, processResult.ValidationResult.HasViolations, validationError.Message);
            Assert.AreEqual("Invalid file structure. Please use approved template!", validationError.Message);
        }

        [Test]
        public void FileProcess_ValidFileProcessInputWithFullAndEmptyRows_ShouldProcessFullRowsAndIngnoreEmpty()
        {
            const string FileName = "AtrendProductTemplate_FullAndSomeEmptyRows.xlsx";
            string testfilePath = Path.Combine(_fileSharePath, "Test Files", FileName);
            var ProductUploader = EngineContext.Current.Resolve<IProductUploader>();
            var stream = new FileStream(testfilePath, FileMode.Open, FileAccess.Read);
            var input = new FileUploadInput(_userContext)
            {
                ContentLength = stream.Length,
                ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = FileName,
                InputStream = stream
            };
            FileUploadResult uploadResult = ProductUploader.FileUpload(input);
            FileProcessResult<ProductInputRow> processResult =
                ProductUploader.FileProcess(new FileProcessInput(_userContext)
                {
                    FileName = uploadResult.TempFileName,
                    FilePath = uploadResult.TempFilePath
                });
            ValidationError validationError = processResult.ValidationResult.ValidationErrors.FirstOrDefault() ??
                                              new ValidationError();

            Assert.AreEqual(false, processResult.ValidationResult.HasViolations, validationError.Message);
            Assert.IsTrue(processResult.FileData.TrueForAll(x => !x.AllStringPropertiesNullOrEmpty()),
                "Should not process empty rows!");
        }

        [Test]
        public void FileProcess_ValidFileProcessInputWithNoRecords_ReturnsErrorResultFileHasNoRecods()
        {
            const string FileName = "AtrendProductTemplate_Empty.xlsx";
            string testfilePath = Path.Combine(_fileSharePath, "Test Files", FileName);
            var ProductUploader = EngineContext.Current.Resolve<IProductUploader>();
            var stream = new FileStream(testfilePath, FileMode.Open, FileAccess.Read);
            var input = new FileUploadInput(_userContext)
            {
                ContentLength = stream.Length,
                ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = FileName,
                InputStream = stream
            };
            FileUploadResult uploadResult = ProductUploader.FileUpload(input);
            FileProcessResult<ProductInputRow> processResult =
                ProductUploader.FileProcess(new FileProcessInput(_userContext)
                {
                    FileName = uploadResult.TempFileName,
                    FilePath = uploadResult.TempFilePath
                });
            ValidationError validationError = processResult.ValidationResult.ValidationErrors.FirstOrDefault() ??
                                              new ValidationError();

            Assert.AreEqual(true, processResult.ValidationResult.HasViolations);
            Assert.AreEqual("No records provided. At least one Product required to initiate upload!",
                validationError.Message);
        }

        [Test]
        public void FileProcess_ValidFileProcessInputWithSomeColumnsInNumericAndSpecialFormat_ShouldProcessAllAsIfTextFormat()
        {
            const string FileName = "AtrendProductTemplate_Small.xlsx";
            string testfilePath = Path.Combine(_fileSharePath, "Test Files", FileName);
            var productUploader = EngineContext.Current.Resolve<IProductUploader>();
            var stream = new FileStream(testfilePath, FileMode.Open, FileAccess.Read);
            var input = new FileUploadInput(_userContext)
            {
                ContentLength = stream.Length,
                ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = FileName,
                InputStream = stream
            };
            FileUploadResult uploadResult = productUploader.FileUpload(input);
            FileProcessResult<ProductInputRow> processResult =
                productUploader.FileProcess(new FileProcessInput(_userContext)
                {
                    FileName = uploadResult.TempFileName,
                    FilePath = uploadResult.TempFilePath
                });
            ValidationError validationError = processResult.ValidationResult.ValidationErrors.FirstOrDefault() ??
                                              new ValidationError();

            Assert.AreEqual(false, processResult.ValidationResult.HasViolations, validationError.Message);
        }

        [Test]
        public void FileProcess_ValidFileProcessInputWithValidRecords_UploadsFileToTempLocation()
        {
            const string FileName = "AtrendProductTemplate_Small.xlsx";
            string testfilePath = Path.Combine(_fileSharePath, "Test Files", FileName);
            var ProductUploader = EngineContext.Current.Resolve<IProductUploader>();
            var stream = new FileStream(testfilePath, FileMode.Open, FileAccess.Read);
            var input = new FileUploadInput(_userContext)
            {
                ContentLength = stream.Length,
                ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = FileName,
                InputStream = stream
            };
            FileUploadResult uploadResult = ProductUploader.FileUpload(input);
            FileProcessResult<ProductInputRow> processResult =
                ProductUploader.FileProcess(new FileProcessInput(_userContext)
                {
                    FileName = uploadResult.TempFileName,
                    FilePath = uploadResult.TempFilePath
                });

            Assert.AreEqual(false, processResult.ValidationResult.HasViolations);
            Assert.IsTrue(processResult.FileData.Any());
        }

        [Test]
        public void FileProcess_ValidFileUploadInputFromExcelFileWithMoreThen10000Rows_ReturnsViolationsDeletesTempFile()
        {
            const string FileName = "AtrendProductTemplate.xlsx";
            const int MaxProductRowsInFile = 10000; // move to config
            const int ActualProductRowsInFile = 10002; // move to config
            string testfilePath = Path.Combine(_fileSharePath, "Test Files", FileName);
            var fileUploader = EngineContext.Current.Resolve<IProductUploader>();
            var stream = new FileStream(testfilePath, FileMode.Open, FileAccess.Read);
            var input = new FileUploadInput(_userContext)
            {
                ContentLength = stream.Length,
                ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = FileName,
                InputStream = stream
            };
            FileUploadResult uploadResult = fileUploader.FileUpload(input);
            FileProcessResult<ProductInputRow> processResult =
                fileUploader.FileProcess(new FileProcessInput(_userContext)
                {
                    FileName = uploadResult.TempFileName,
                    FilePath = uploadResult.TempFilePath
                });
            ValidationError validationError = processResult.ValidationResult.ValidationErrors.FirstOrDefault() ??
                                              new ValidationError();

            Assert.AreEqual(true, processResult.ValidationResult.HasViolations);
            Assert.AreEqual(
                string.Format(
                    "Batch size has been exceeded. Maximum allowed size is {0} rows, file provided has: {1} rows",
                    MaxProductRowsInFile, ActualProductRowsInFile), validationError.Message);
            Assert.AreEqual(false, File.Exists(uploadResult.TempFilePath), "File must be deleted!");
        }

        [Test]
        public void FileUpload_InValidFileUploadInputExcelWithoutSchema_ReturnsErrorResultInvalidFileFormat()
        {
            const string FileName = "RandomExcelFile.xlsx";
            string testfilePath = Path.Combine(_fileSharePath, "Test Files", FileName);
            var fileUploader = EngineContext.Current.Resolve<IProductUploader>();
            var stream = new FileStream(testfilePath, FileMode.Open, FileAccess.Read);
            var input = new FileUploadInput(_userContext)
            {
                ContentLength = stream.Length,
                ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = FileName,
                InputStream = stream
            };
            FileUploadResult uploadResult = fileUploader.FileUpload(input);
            ValidationError validationError = uploadResult.ValidationResult.ValidationErrors.FirstOrDefault() ??
                                              new ValidationError();

            Assert.AreEqual(true, uploadResult.ValidationResult.HasViolations);
            Assert.AreEqual("Invalid file structure. Please use approved template!", validationError.Message);
        }

        [Test]
        public void FileUpload_NullInput_ThorwsException()
        {
            var fileUploader = EngineContext.Current.Resolve<IProductUploader>();

            Assert.Throws<ArgumentNullException>(() => fileUploader.FileUpload(null));
        }

        [Test]
        public void FileUpload_ValidFileUploadInputFromExcelFile_UploadsFileToTempLocation()
        {
            const string fileName = "AtrendProductTemplate.xlsx";
            string tempFileDirectoryPath = Path.Combine(_fileSharePath, "Temp");
            string testfilePath = Path.Combine(_fileSharePath, "Test Files", fileName);
            var fileUploader = EngineContext.Current.Resolve<IProductUploader>();
            var stream = new FileStream(testfilePath, FileMode.Open, FileAccess.Read);
            var input = new FileUploadInput(_userContext)
            {
                ContentLength = stream.Length,
                ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = fileName,
                InputStream = stream
            };
            FileUploadResult uploadResult = fileUploader.FileUpload(input);

            Assert.AreEqual(false, uploadResult.ValidationResult.HasViolations);
            Assert.IsTrue(uploadResult.TempFileName.Contains(fileName));
            Assert.IsTrue(uploadResult.TempFilePath.Contains(tempFileDirectoryPath));
        }

        [Test]
        public void FileUpload_ValidFileUploadInputFromNotExcelFile_ReturnsErrorResultFormatNotSupported()
        {
            var fileUploader = EngineContext.Current.Resolve<IProductUploader>();
            var input = new FileUploadInput(_userContext)
            {
                ContentType = "JPG"
            };
            FileUploadResult uploadResult = fileUploader.FileUpload(input);
            ValidationError validationError = uploadResult.ValidationResult.ValidationErrors.FirstOrDefault() ??
                                              new ValidationError();

            Assert.AreEqual(true, uploadResult.ValidationResult.HasViolations);
            Assert.AreEqual("JPG format is not supported!", validationError.Message);
        }

        [Test]
        public void OutputFileProcess_ValidTempFileProcessInputRecords_GeneratesOutputFile()
        {
            const string FileName = "AtrendProductTemplate_Small.xlsx";
            string testfilePath = Path.Combine(_fileSharePath, "Test Files", FileName);
            var stream = new FileStream(testfilePath, FileMode.Open, FileAccess.Read);
            var input = new FileUploadInput(_userContext)
            {
                ContentLength = stream.Length,
                ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = FileName,
                InputStream = stream
            };

            var dummyProcessedData = new List<ProductInputRow>
            {
                new ProductInputRow
                {
                    RowIndex = 1,
                    UploadedProductId = Guid.NewGuid().ToString(),
                    UploadStatus = "Success",
                    UploadMessage = "Success Message"
                }
            };

            var productUploader = EngineContext.Current.Resolve<IProductUploader>();
            FileUploadResult uploadResult = productUploader.FileUpload(input);

            OutputFileProcessResult outputResult =
                productUploader.OutputFileProcess(new OutputFileProcessInput<ProductInputRow>(_userContext)
                {
                    TempFileName = uploadResult.TempFileName,
                    TempFilePath = uploadResult.TempFilePath,
                    ProcessedFileData = dummyProcessedData
                });

            Assert.AreEqual(false, outputResult.ValidationResult.HasViolations);
            Assert.IsTrue(File.Exists(outputResult.OutputFileInfo.FilePath));
        }

        #endregion

        #region IProductUploader tests

        [Test]
        public void Upload_ValidFileUploadInputFromExcelFile_UploadsProductDataFromTheFileUpdatesStatus()
        {
            const string FileName = "Web Map-Reikken (Autosaved) (1) - Copy.xlsx";
            var traceToken = Guid.NewGuid().ToString();
            var testfilePath = Path.Combine(_fileSharePath, "Test Files", FileName);
            var productUploader = EngineContext.Current.Resolve<IProductUploader>();
            var stream = new FileStream(testfilePath, FileMode.Open, FileAccess.Read);
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var input = new FileUploadInput(_userContext)
            {
                UploadTraceToken = traceToken,
                ContentLength = stream.Length,
                ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = FileName,
                InputStream = memoryStream
            };
            productUploader.Upload(input);
            var uploadStatus = productUploader.GetUploadStatus(traceToken);
            var uploadHistory = productUploader.GetHistory(5, 0);

            Assert.IsNotNull(uploadStatus);
            Assert.IsNotNull(uploadHistory);
            foreach (var uploadStage in uploadStatus.UploadStageTrace)
            {
                Console.WriteLine("{0} : {1}", uploadStage.Stage, uploadStage.Message);
            }
            foreach (var fileInfo in uploadHistory)
            {
                Console.WriteLine("{0} : {1}", fileInfo.UploadDate, fileInfo.FilePath);
            }
        }

        //todo create factory
        [Test]
        public void GetHistory_ReturnsListOfUploadedFiles()
        {
            var productUploader = EngineContext.Current.Resolve<IProductUploader>();

            const int ToTake = 5;
            const int ToSkip = 0;
            var history = productUploader.GetHistory(ToTake, ToSkip);

            Assert.AreEqual(ToTake, history.Count);
        }

        #endregion
    }
}