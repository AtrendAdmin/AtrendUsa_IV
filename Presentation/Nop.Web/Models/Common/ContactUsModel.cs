using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Validators.Common;

namespace Nop.Web.Models.Common
{
    [Validator(typeof(ContactUsValidator))]
    public partial class ContactUsModel : BaseNopModel
    {
        [AllowHtml]
        [NopResourceDisplayName("ContactUs.Email")]
        public string Email { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.Subject")]
        public string Subject { get; set; }
        public bool SubjectEnabled { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.Enquiry")]
        public string Enquiry { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.FullName")]
        public string FullName { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.ActgEmail")]
        public string ActgEmail { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.BuyerEmail")]
        public string BuyerEmail { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.AdditionalComments")]
        public string AdditionalComments { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.FirstName")]
        public string FirstName { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.LastName")]
        public string LastName { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.CompanyName")]
        public string CompanyName { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.BusinessAddress")]
        public string BusinessAddress { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.HomeAddress")]
        public string HomeAddress { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.Title")]
        public string Title { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.City")]
        public string City { get; set; }

        [NopResourceDisplayName("ContactUs.State")]
        public string StateProvince { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.ZipCode")]
        public string ZipCode { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.PhoneNumber")]
        public string BusinessPhoneNumber { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.Fax")]
        public string FaxNumber { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.HomePhoneNumber")]
        public string HomePhoneNumber { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ConcactUs.AccountingContact")]
        public string AccountingContact { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.TypeOfBusiness")]
        public string TypeOfBusiness { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("ContactUs.Established")]
        public string Established { get; set; }

        [NopResourceDisplayName("ContactUs.Country")]
        public string Country { get; set; }

        [NopResourceDisplayName("ContactUs.KeyInterest")]
        public string KeyInterest { get; set; }
    }
}