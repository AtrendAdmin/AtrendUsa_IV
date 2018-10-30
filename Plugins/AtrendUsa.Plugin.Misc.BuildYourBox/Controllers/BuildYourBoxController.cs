using System.IO;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Services.Directory;
using Nop.Web.Framework.Controllers;
using Nop.Services.Localization;
using Nop.Web.Framework.Security.Captcha;
using Nop.Services.Topics;
using AtrendUsa.Plugin.Misc.BuildYourBox.Models;
using AtrendUsa.Plugin.Misc.BuildYourBox.Services;
//using Nop.Core.Plugins;
//using AtrendUsa.Plugin.Misc.Support.Services;
using Nop.Services.BuildYourBoxPlugin;
using System;

namespace AtrendUsa.Plugin.Misc.BuildYourBox.Controllers
{
    public class BuildYourBoxController : BasePluginController
    {
        private readonly int _maxAttachmentsSize = 25 * 1024 * 1024; // 25 MB
        private readonly string[] _allowedAttachmentTypes = new[] { "pdf", "jpeg", "jpg", "gif", "png", "bmp" };

        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ILocalizationService _localizationService;
        private readonly ICountryService _countryService;
        private readonly CaptchaSettings _captchaSettings;
        private readonly IBuildYourBoxMessageService _messageService;
        private readonly ITopicService _topicService;
        private readonly IBuildYourBoxService _buildYourBoxService;
        private readonly ICarpetColorService _carpetColorService;
        private readonly ITerminalTypeService _terminalTypeService;
        private readonly IMDFThicknessService _mDFThicknessService;

        public BuildYourBoxController(
           IWorkContext workContext,
           IStoreContext storeContext,
           ILocalizationService localizationService,
           ICountryService countryService,
           CaptchaSettings captchaSettings,
           IBuildYourBoxMessageService messageService,
           ITopicService topicService,
           IBuildYourBoxService buildYourBoxService,
           ICarpetColorService CarpetColorService,
           ITerminalTypeService TerminalTypeService,
           IMDFThicknessService MDFThicknessService)
        {
            _workContext = workContext;
            _storeContext = storeContext;
            _localizationService = localizationService;
            _countryService = countryService;
            _captchaSettings = captchaSettings;
            _messageService = messageService;
            _topicService = topicService;
            _buildYourBoxService = buildYourBoxService;
            _carpetColorService = CarpetColorService;
            _terminalTypeService = TerminalTypeService;
            _mDFThicknessService = MDFThicknessService;

            FillTabsVisibilityValues();
        }

        public void FillTabsVisibilityValues()
        {
            ViewBag.availableServiceCentersTabVisibility = (_topicService.GetTopicBySystemName("AvailableServiceCenters", _storeContext.CurrentStore.Id) != null);
            ViewBag.storeName = _storeContext.CurrentStore.Name;
        }
        public ActionResult Index()
        {
            return RedirectToAction("YourBoxForm", "BuildYourBox"); //Added by IV Santosh
            //return View("~/Plugins/AtrendUsa.Plugin.Misc.BuildYourBox/Views/YourBoxForm.cshtml");
        }

        public ActionResult YourBoxForm()
        {
            BuildYourBoxModel model = new BuildYourBoxModel();
            PrepareBuildYourBoxModel(model);
            //return RedirectToAction("ClaimForm", "Support"); //Added by IV Santosh
            return View("~/Plugins/AtrendUsa.Plugin.Misc.BuildYourBox/Views/YourBoxForm.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidator]
        public ActionResult YourBoxForm(BuildYourBoxModel modelData, bool captchaValid = false)
        {
            ValidateCaptcha(captchaValid);
            BuildYourBoxModel model = new BuildYourBoxModel();
            PrepareBuildYourBoxModel(model);
            if (ModelState.IsValid)
            {
                _messageService.SendBuildYourBoxMessage(modelData);
                //var MDFThicknessRequested = Enum.GetName(typeof(MDFThicknessRequested), modelData.MDFThicknessRequested);
                var buildYourBox = new Nop.Core.Domain.BuildYourBoxPlugin.BuildYourBox()
                {
                    SubwooferManufacturer = modelData.SubwooferManufacturer,
                    SubwooferModel = modelData.SubwooferModel,
                    NumberOfSubwoofers = modelData.NumberOfSubwoofers,
                    VehicleManufacturerOrModel = modelData.VehicleManufacturerOrModel,
                    VehicleYear = modelData.VehicleYear,
                    WidthRequired = modelData.WidthRequired,
                    HeightRequired = modelData.HeightRequired,
                    DepthRequired = modelData.DepthRequired,
                    CarpetColorId = modelData.CarpetColorId ?? 0,
                    TerminalLocation = modelData.TerminalLocation,
                    TerminalTypeId = modelData.TerminalTypeId ?? 0,
                    SealedOrVented = modelData.SealedOrVented,
                    MDFThicknessId = modelData.MDFThicknessId ?? 0,
                    SquareOrAngledBox = modelData.SquareOrAngledBox,
                    Name = modelData.Name,
                    Address = modelData.Address,
                    PhoneNumber = modelData.PhoneNumber,
                    Email = modelData.Email
                };
                _buildYourBoxService.InsertBuildYourBox(buildYourBox);

                model.SuccessfullySent = true;
                model.Result = "Your Build Box was successfully sent";
            }
            return View("~/Plugins/AtrendUsa.Plugin.Misc.BuildYourBox/Views/YourBoxForm.cshtml", model);
        }

        public ActionResult DDLToolTipTest()
        {
            return View("~/Plugins/AtrendUsa.Plugin.Misc.BuildYourBox/Views/DDLToolTipTest.cshtml");
        }

        public void PrepareBuildYourBoxModel(BuildYourBoxModel model)
        {
            //model.DisplayCaptcha = false;
            model.DisplayCaptcha = _captchaSettings.Enabled;
            model.AllowedAttachmentsTypes = _allowedAttachmentTypes;
            model.MaxAllowedAttachmentsSize = _maxAttachmentsSize;
            FillCarpetColor(model);
            FillTerminalType(model);
            FillMDFThickness(model);
        }

        public void FillCarpetColor(BuildYourBoxModel model)
        {
            model.CarpetColorWanted.Add(new SelectListItem { Text = _localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.SelectCarpetColor"), Value = "" });
            var carpetColors = _carpetColorService.SelectAllCarpetColor();
            ViewBag.CarpetColors = carpetColors;
            
            foreach (var item in carpetColors)
            {
                model.CarpetColorWanted.Add(new SelectListItem { Text = item.CarpetColor, Value = Convert.ToString(item.Id) });
            }
        }

        public void FillTerminalType(BuildYourBoxModel model)
        {
            model.TerminalType.Add(new SelectListItem { Text = _localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.SelectTerminalType"), Value = "" });
            var terminalTypes = _terminalTypeService.SelectAllTerminalType();
            ViewBag.TerminalTypes = terminalTypes;
            
            foreach (var item in terminalTypes)
            {
                model.TerminalType.Add(new SelectListItem { Text = item.TerminalType, Value = Convert.ToString(item.Id) });
            }
        }

        public void FillMDFThickness(BuildYourBoxModel model)
        {
            model.MDFThicknessRequested.Add(new SelectListItem { Text = _localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.SelectMDFThickness"), Value = "" });
            var mDFThickness = _mDFThicknessService.SelectAllMDFThicknesses();
            foreach (var item in mDFThickness)
            {
                model.MDFThicknessRequested.Add(new SelectListItem { Text = item.MDFThickness, Value = Convert.ToString(item.Id) });
            }
        }
        private void ValidateCaptcha(bool captchaValid)
        {
            if (!captchaValid)
            {
                ModelState.AddModelError("", "Captcha is invalid");
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
