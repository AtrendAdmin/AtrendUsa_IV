using System.IO;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Services.Directory;
using Nop.Web.Framework.Controllers;
using Nop.Services.Localization;
using Nop.Web.Framework.Security.Captcha;
using Nop.Services.Topics;
//using AtrendUsa.Plugin.Misc.Support.Services;

namespace AtrendUsa.Plugin.Misc.BuildYourBox.Controllers
{
    public class BuildYourBoxController : BasePluginController
    {
        private readonly int _maxAttachmentsSize = 25 * 1024 * 1024; // 25 MB
        private readonly string[] _allowedAttachmentTypes = new[] { "doc", "docx", "xls", "xlsx", "ods", "pdf", "jpeg", "jpg", "gif", "png", "bmp" };

        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ILocalizationService _localizationService;
        private readonly ICountryService _countryService;
        private readonly CaptchaSettings _captchaSettings;
        //private readonly ISupportMessageService _messageService;
        private readonly ITopicService _topicService;

        public BuildYourBoxController(
           IWorkContext workContext,
           IStoreContext storeContext,
           ILocalizationService localizationService,
           ICountryService countryService,
           CaptchaSettings captchaSettings,
           //ISupportMessageService messageService,
           ITopicService topicService)
        {
            _workContext = workContext;
            _storeContext = storeContext;
            _localizationService = localizationService;
            _countryService = countryService;
            _captchaSettings = captchaSettings;
            //_messageService = messageService;
            _topicService = topicService;

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
            //return RedirectToAction("ClaimForm", "Support"); //Added by IV Santosh
            return View("~/Plugins/AtrendUsa.Plugin.Misc.BuildYourBox/Views/YourBoxForm.cshtml");
        }

    }
}
