using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtrendUsa.Plugin.Misc.Support.Validators;
using FluentValidation.Attributes;
using Nop.Web.Framework;

namespace AtrendUsa.Plugin.Misc.Support.Models
{
    [Validator(typeof(ReturnAuthorizationRequestModelValidator))]
    public class ReturnAuthorizationRequestModel : BaseSupportModel
    {
        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.RA.ModelNumber")]
        public string ModelNumber { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.RA.SerialNumber")]
        public string SerialNumber { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.UnderWarranty")]
        public bool IsUnderWarranty { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.DatePurchased")]
        public string DatePurchased { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.Dealer")]
        public string Dealer { get; set; }
    }
}
