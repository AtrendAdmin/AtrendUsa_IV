using System.Web.Mvc;

namespace AtrendUsa.Plugin.Admin.Uploader.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(string errorIdentifier)
        {
            ViewData["ErrorIdentifier"] = errorIdentifier;
            return View("Error");
        }

        public JsonResult AjaxError(string errorIdentifier)
        {
            return Json(
                new
                {
                    StatusCode = 500,
                    Message = "Sorry, an error occurred while processing your request. Please contact the Service Desk. Reference code: " + errorIdentifier
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}