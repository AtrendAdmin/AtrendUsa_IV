using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AtrendUsa.Plugin.Misc.Support.Models;
using AtrendUsa.Plugin.Misc.Support.Services;
using Nop.Core;
using Nop.Services.Directory;
using Nop.Web.Framework.Controllers;
using Nop.Services.Localization;
using Nop.Web.Framework.Security.Captcha;
using Nop.Services.Topics;

namespace AtrendUsa.Plugin.Misc.Support.Controllers
{
    public class SupportController : BasePluginController
    {
        private readonly int _maxAttachmentsSize = 25 * 1024 * 1024; // 25 MB
        private readonly string[] _allowedAttachmentTypes = new[] { "doc", "docx", "xls", "xlsx", "ods", "pdf", "jpeg", "jpg", "gif", "png", "bmp" };

        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ILocalizationService _localizationService;
        private readonly ICountryService _countryService;
        private readonly IModelNumberService _modelNumberService;       //Added by IV Santosh 
        private readonly CaptchaSettings _captchaSettings;
        private readonly ISupportMessageService _messageService;
        private readonly ITopicService _topicService;

        public SupportController(
            IWorkContext workContext,
            IStoreContext storeContext,
            ILocalizationService localizationService,
            ICountryService countryService,
            IModelNumberService modelNumberService,         //Added by IV Santosh 
            CaptchaSettings captchaSettings,
            ISupportMessageService messageService,
            ITopicService topicService)
        {
            _workContext = workContext;
            _storeContext = storeContext;
            _localizationService = localizationService;
            _countryService = countryService;
            _modelNumberService = modelNumberService;       //Added by IV Santosh 
            _captchaSettings = captchaSettings;
            _messageService = messageService;
            _topicService = topicService;

            FillTabsVisibilityValues();
        }

        public ActionResult Index()
        {
            //return RedirectToAction("ReturnAuthorizationRequest"); //Commented by IV Santosh
            return RedirectToAction("FreightOrderClaim", "Support"); //Added by IV Santosh
        }

        public ActionResult ReturnAuthorizationRequest()
        {
            var model = new ReturnAuthorizationRequestModel();
            PrepareSupportModel(model);
            return View("~/Plugins/AtrendUsa.Plugin.Misc.Support/Views/ReturnAuthorizationRequest.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidator]
        public ActionResult ReturnAuthorizationRequest(ReturnAuthorizationRequestModel model, bool captchaValid)
        {
            PrepareSupportModel(model);
            ValidateCaptcha(captchaValid);
            ValidateAttachments(model);

            if (ModelState.IsValid)
            {
                _messageService.SendReturnAuthorizationRequestMessage(model);
                model.SuccessfullySent = true;
                model.Result = "Your request was successfully sent";
            }

            return View("~/Plugins/AtrendUsa.Plugin.Misc.Support/Views/ReturnAuthorizationRequest.cshtml", model);
        }

        public ActionResult FreightOrderClaim()
        {
            var model = new FreightOrderClaimModel();
            PrepareSupportModel(model);
            return View("~/Plugins/AtrendUsa.Plugin.Misc.Support/Views/FreightOrderClaim.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[CaptchaValidator] //Commented by IV Santosh 
        public ActionResult FreightOrderClaim(FreightOrderClaimModel model, bool captchaValid = true) //Added CaptchValid = False before CaptchValid IV Santosh
        {
            PrepareSupportModel(model);
            //ValidateCaptcha(captchaValid); //Commented by IV Santosh
            ValidateAttachments(model);

            if (ModelState.IsValid)
            {
                if (model.ModelNumberId != null)
                {
                    model.ModelNumber = string.Join(",", model.ModelNumberId);
                }
                _messageService.SendFreightOrderClaimMessage(model); //Commented by IV Santosh
                model.SuccessfullySent = true;
                model.Result = "Your claim was successfully sent";
            }
            return View("~/Plugins/AtrendUsa.Plugin.Misc.Support/Views/FreightOrderClaim.cshtml", model);
        }

        public ActionResult Download()
        {
            return View("~/Plugins/AtrendUsa.Plugin.Misc.Support/Views/Download.cshtml");
        }

        public ActionResult Warranty()
        {
            return View("~/Plugins/AtrendUsa.Plugin.Misc.Support/Views/Warranty.cshtml");
        }

        public ActionResult AvailableServiceCenters()
        {
            if (!ViewBag.availableServiceCentersTabVisibility)
                return HttpNotFound();

            return View("~/Plugins/AtrendUsa.Plugin.Misc.Support/Views/AvailableServiceCenters.cshtml");
        }

        public void FillTabsVisibilityValues()
        {
            ViewBag.availableServiceCentersTabVisibility = (_topicService.GetTopicBySystemName("AvailableServiceCenters", _storeContext.CurrentStore.Id) != null);
            ViewBag.storeName = _storeContext.CurrentStore.Name;
        }

        private void PrepareSupportModel(BaseSupportModel model)
        {
            //model.DisplayCaptcha = _captchaSettings.Enabled; // Commented by IV Santosh
            model.DisplayCaptcha = false; //Added by IV Santosh
            model.StoreName = _storeContext.CurrentStore.Name;
            model.AllowedAttachmentsTypes = _allowedAttachmentTypes;
            model.MaxAllowedAttachmentsSize = _maxAttachmentsSize;
            FillCountries(model);
            FillModelNumbers(model); /*Added By IV Santosh*/
        }

        private void ValidateCaptcha(bool captchaValid)
        {
            if (!captchaValid)
            {
                ModelState.AddModelError("", "Captcha is invalid");
            }
        }

        private void ValidateAttachments(BaseSupportModel model)
        {
            var attachments = model.UploadedDocuments.Where(x => x != null);
            if (attachments.Any())
            {
                int uploadSize = attachments.Sum(x => x.ContentLength);
                if (uploadSize > _maxAttachmentsSize)
                {
                    ModelState.AddModelError("", $"Max allowed attachments size is {_maxAttachmentsSize / 1024 / 1024} MBytes");
                }

                var fileTypes = attachments.Select(x => Path.GetExtension(x.FileName).Replace(".", string.Empty));
                foreach (var ext in fileTypes)
                {
                    if (string.IsNullOrEmpty(ext) || !_allowedAttachmentTypes.Contains(ext.ToLower()))
                    {
                        ModelState.AddModelError("", "Allowed attachments types are: " + string.Join(", ", _allowedAttachmentTypes));
                        break;
                    }
                }
            }
        }

        private void FillCountries(BaseSupportModel model)
        {
            model.AvailableCountries.Add(new SelectListItem() { Text = _localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.SelectCountry"), Value = "0" });

            foreach (var c in _countryService.GetAllCountries(_workContext.WorkingLanguage.Id, true))
            {
                model.AvailableCountries.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            }
        }


        private void FillModelNumbers(BaseSupportModel model)        //Added by IV Santosh 
        {
            //model.AvailableModelNumbers.Add(new SelectListItem() { Text = _localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.SelectModelNumber"), Value = "0" });
            //model.AvailableModelNumbers.Add(new SelectListItem() { Text = "Select Model Numbers", Value = "0" });

            foreach (var c in _modelNumberService.GetAllModelNumbers(_workContext.WorkingLanguage.Id, true))
            {
                model.AvailableModelNumbers.Add(new SelectListItem() { Text = c.ModelNum, Value = c.ModelNum.ToString() });
            }
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            return Content("");
        }
    }
}
