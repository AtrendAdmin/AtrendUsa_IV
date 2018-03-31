using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AtrendUsa.Plugin.Admin.Uploader.Services.Extensions;
using AtrendUsa.Plugin.Admin.Uploader.Services.Interfaces;
using AtrendUsa.Plugin.Admin.Uploader.Services.Model;
using AtrendUsa.Plugin.Admin.Uploader.ViewModel;
using Nop.Admin.Controllers;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Logging;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;

namespace AtrendUsa.Plugin.Admin.Uploader.Controllers
{
    [AdminAuthorize]
    public class UploadController : BaseAdminController
    {
        private static readonly string FileSharePath = ConfigurationManager.AppSettings["FileShare"];
        private static readonly string TemplateDirPath = ConfigurationManager.AppSettings["TemplateDir"];
        private static readonly string TemplateName = ConfigurationManager.AppSettings["TemplateName"];
        private static readonly string FileShareProcessedFilesPath = Path.Combine(FileSharePath, "Processed");


        private readonly IProductUploader _productUploader;
        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;

        public UploadController(IProductUploader productUploader, ILogger logger, IWorkContext workContext, IPermissionService permissionService)
        {
            _productUploader = productUploader;
            _logger = logger;
            _workContext = workContext;
            _permissionService = permissionService;
        }

        public ActionResult Upload()
        {
            _logger.Debug("Upload init");

            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor cannot import products
            if (_workContext.CurrentVendor != null)
                return AccessDeniedView();

            var viewModel = new UploadViewModel { };

            return View("~/Plugins/AtrendUsa.Plugin.Admin.Uploader/Views/Upload/Upload.cshtml");
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor cannot import products
            if (_workContext.CurrentVendor != null)
                return AccessDeniedView();
            return View("~/Plugins/AtrendUsa.Plugin.Admin.Uploader/Views/Upload/UploadIframe.cshtml", new UploadFileViewModel { UploadTraceToken = string.Empty });
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase inputFile)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //a vendor cannot import products
            if (_workContext.CurrentVendor != null)
                return AccessDeniedView();

            if (inputFile == null) throw new ArgumentNullException("inputFile");
            _logger.Debug("Upload file");
            using (inputFile.InputStream)
            {
                // doing it because IE8 sends in full path instead of name
                var fileName = Path.GetFileName(inputFile.FileName);
                var userContext = _workContext.GetUserContext();
                var uploadTraceToken = Guid.NewGuid().ToString();
                // copy to memory and pass it along
                var memoryStream = new MemoryStream();
                inputFile.InputStream.CopyTo(memoryStream);
                memoryStream.Position = 0;

                var fileUploadInput = new FileUploadInput(userContext)
                    {
                        UploadTraceToken = uploadTraceToken,
                        ContentLength = inputFile.ContentLength,
                        FileName = fileName,
                        InputStream = memoryStream,
                        ContentType = inputFile.ContentType
                    };

                Task.Factory.StartNew(() => EngineContext.Current.Resolve<IProductUploader>().Upload(fileUploadInput));

                return View("~/Plugins/AtrendUsa.Plugin.Admin.Uploader/Views/Upload/UploadIframe.cshtml", new UploadFileViewModel { UploadTraceToken = uploadTraceToken });
            }
        }

        [HttpPost]
        public JsonResult GetUploadStatus(string traceToken)
        {
            return Json(_productUploader.GetUploadStatus(traceToken));
        }

        [HttpPost]
        public JsonResult GetUploadHistory(int take, int skip)
        {
            return Json(_productUploader.GetHistory(take, skip));
        }


        [HttpGet]
        public ActionResult DownloadUploadedFile(string fileName)
        {
            var folder = fileName.Split('_')[0];
            var filePath = Path.Combine(FileShareProcessedFilesPath, folder, fileName);

            return File(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet]
        public ActionResult DownloadTemplateFile()
        {
            var templateFilePath = Path.Combine(TemplateDirPath, TemplateName);
            return File(templateFilePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", TemplateName);
        }


        #region private properties

        #endregion
    }
}