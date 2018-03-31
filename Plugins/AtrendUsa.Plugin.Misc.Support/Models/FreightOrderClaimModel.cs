using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AtrendUsa.Plugin.Misc.Support.Validators;
using Nop.Web.Framework;
using FluentValidation.Attributes;

namespace AtrendUsa.Plugin.Misc.Support.Models
{
    public enum ExceptionType
    {
        [Display(Name = "Damaged Goods")]
        DamagedGoods,
        [Display(Name = "Shortages")]
        Shortages,
        [Display(Name = "Warranty Issues")]
        WarrantyIssues,
        [Display(Name = "Overages")] // Added by IV Santosh
        Overages // Added by IV Santosh
    }

    [Validator(typeof(FreightOrderClaimModelValidator))]
    public class FreightOrderClaimModel : BaseSupportModel
    {
        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.FO.ModelNumber")]
        public string ModelNumber { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.FO.SerialNumber")]
        public string SerialNumber { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.Support.ExceptionType")]
        public ExceptionType? ExceptionType { get; set; }

        public string ExceptionTypeName
        {
            get
            {
                var type = ExceptionType;

                switch (type)
                {
                    case null:
                        return "Unknown";
                    case Models.ExceptionType.DamagedGoods:
                        return "Damaged Goods";
                    case Models.ExceptionType.Shortages:
                        return "Shortages";
                    case Models.ExceptionType.WarrantyIssues:
                        return "Warranty Issues";
                    case Models.ExceptionType.Overages:
                        return "Overages";
                }
                return string.Empty;
            }
        }
    }
}
