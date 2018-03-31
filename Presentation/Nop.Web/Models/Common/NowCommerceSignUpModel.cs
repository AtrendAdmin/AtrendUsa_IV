using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Validators.Common;

namespace Nop.Web.Models.Common
{
    [Validator(typeof(NowCommerceSignUpValidator))]
    public partial class NowCommerceSignUpModel : BaseNopModel
    {
        [NopResourceDisplayName("NowCommerceSignUp.DealerName")]
        public string DealerName { get; set; }

        [NopResourceDisplayName("NowCommerceSignUp.ContactPerson")]
        public string ContactPerson { get; set; }

        [NopResourceDisplayName("NowCommerceSignUp.ContactEmail")]
        public string ContactEmail { get; set; }

        [NopResourceDisplayName("NowCommerceSignUp.AdditionalComments")]
        public string AdditionalComments { get; set; }

        public bool DisplayCaptcha { get; set; }

        public bool SuccessfullySent { get; set; }

        public string Result { get; set; }
    }
}