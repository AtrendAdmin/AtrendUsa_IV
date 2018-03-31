using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Nop.Web.Framework;

namespace AtrendUsa.Plugin.Misc.Support.Models
{
    public class BaseSupportModel
    {
        public BaseSupportModel()
        {
            AvailableCountries = new List<SelectListItem>();
            UploadedDocuments = new List<HttpPostedFileBase>();
            AvailableModelNumbers = new List<SelectListItem>();
        }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.CompanyName")] //Added by IV Santosh
        public string CompanyName { get; set; } //Added by IV Santosh

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.FirstName")]
        public string FirstName { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.LastName")]
        public string LastName { get; set; }

        public string FullName => FirstName + " " + LastName;

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.Email")]
        public string Email { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.Country")]
        public int? CountryId { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.StateProvince")]
        public string StateProvince { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.City")]
        public string City { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.AddressLine1")]
        public string AddressLine1 { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.AddressLine2")]
        public string AddressLine2 { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.PostalCode")]
        public string PostalCode { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.Phone")]
        public string Phone { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.Problem")]
        public string Problem { get; set; }

        [DataType(DataType.Upload)]
        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.UploadedDocuments")]
        public IEnumerable<HttpPostedFileBase> UploadedDocuments { get; set; }

        public bool DisplayCaptcha { get; set; }

        public bool SuccessfullySent { get; set; }

        public string Result { get; set; }

        public string StoreName { get; set; }

        public int MaxAllowedAttachmentsSize { get; set; }

        public string[] AllowedAttachmentsTypes { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.ModelNumber")] //Added by IV Santosh
        public string[] ModelNumberId { get; set; } //Added by IV Santosh
        public IList<SelectListItem> AvailableModelNumbers { get; set; } //Added by IV Santosh
    }
}
