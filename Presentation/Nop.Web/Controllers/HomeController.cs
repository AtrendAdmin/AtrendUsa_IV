using System;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Stores;
using Nop.Web.Factories;
using Nop.Web.Framework.Security;

namespace Nop.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IStoreService _storeService;
        private readonly IStoreContext _storeContext;

        public HomeController(ICommonModelFactory commonModelFactory, IStoreService storeService, IStoreContext storeContext)
        {
            _commonModelFactory = commonModelFactory;
            _storeService = storeService;
            _storeContext = storeContext;
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public virtual ActionResult Index()
        {
            var isStorefront = Request.QueryString.Get("sf");

            if (isStorefront == "true")
            {
                var model = _commonModelFactory.PrepareSocialModel();

                return View("Storefront", model);
            }

            var urlReferrer = Convert.ToString(Request.UrlReferrer);
            var currentStore = _storeContext.CurrentStore;
            var stores = _storeService.GetAllStores();
            var mainStore = stores.First();

            if (currentStore.Url.IndexOf(mainStore.Url, StringComparison.OrdinalIgnoreCase) == 0 && urlReferrer.IndexOf(mainStore.Url, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return Redirect(mainStore.Url + "?sf=true");
            }

            return View();
        }
    }
}
