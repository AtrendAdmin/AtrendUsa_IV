using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using FluentValidation.Attributes;

namespace AtrendUsa.Plugin.Misc.Support.Models
{
    [Validator(typeof(SupportModelValidator))]
    public class SupportModel
    {
        public SupportModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
            AvailableManufacturers = new List<SelectListItem>();
        }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.FirstName")]
        public string FirstName { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.LastName")]
        public string LastName { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.Email")]
        public string Email { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.AddressLine1")]
        public string AddressLine1 { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.AddressLine2")]
        public string AddressLine2 { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.City")]
        public string City { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.StateProvince")]
        public string StateProvince { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.PostalCode")]
        public string PostalCode { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.Phone")]
        public string Phone { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.Country")]
        public int? CountryId { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.ModelNumber")]
        public string ModelNumber { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.UnderWarranty")]
        public bool IsUnderWarranty { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.DatePurchased")]
        public string DatePurchased { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.SerialNumber")]
        public string SerialNumber { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.Category")]
        public int? CategoryId { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.Manufacturer")]
        public int? ManufacturerId { get; set; }
        public IList<SelectListItem> AvailableManufacturers { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.Dealer")]
        public string Dealer { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.Problem")]
        public string Problem { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }
    }
}
