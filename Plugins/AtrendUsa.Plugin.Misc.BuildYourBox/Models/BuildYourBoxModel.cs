using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using AtrendUsa.Plugin.Misc.BuildYourBox.Validators;
using Nop.Web.Framework;
using System.Web.Mvc;
using System.Web;
using Nop.Services.BuildYourBoxPlugin;

namespace AtrendUsa.Plugin.Misc.BuildYourBox.Models
{
    [Validator(typeof(BuildYourBoxModelValidator<BuildYourBoxModel>))]
    public class BuildYourBoxModel
    {
        public BuildYourBoxModel()
        {
            CarpetColorWanted = new List<SelectListItem>();
            ImageOfVehicleTrunkArea = new List<HttpPostedFileBase>();
            TerminalType = new List<SelectListItem>();
            MDFThicknessRequested = new List<SelectListItem>();
        }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.SubwooferManufacturer")]
        public string SubwooferManufacturer { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.SubwooferModel")]
        public string SubwooferModel { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.NumberOfSubwoofers")]
        public int NumberOfSubwoofers { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.VehicleManufacturerOrModel")]
        public string VehicleManufacturerOrModel { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.VehicleYear")]
        public string VehicleYear { get; set; }

        //[DataType(DataType.Upload)]
        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.ImageOfVehicleTrunkArea")]
        public IEnumerable<HttpPostedFileBase> ImageOfVehicleTrunkArea { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.Width")]
        public string WidthRequired { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.Height")]
        public string HeightRequired { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.Depth")]
        public string DepthRequired { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.CarpetColor")]
        public int? CarpetColorId { get; set; }
        public IList<SelectListItem> CarpetColorWanted { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.TerminalLocation")]
        public string TerminalLocation { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.TerminalType")]
        public int? TerminalTypeId { get; set; }
        public IList<SelectListItem> TerminalType { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.SealedOrVented")]
        public string SealedOrVented { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.MDFThicknessRequested")]
        public int? MDFThicknessId { get; set; }
        public IList<SelectListItem> MDFThicknessRequested { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.SquareOrAngledBox")]
        public string SquareOrAngledBox { get; set; }

        public bool SuccessfullySent { get; set; }

        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }

        public int MaxAllowedAttachmentsSize { get; set; }

        public string[] AllowedAttachmentsTypes { get; set; }

        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.Name")]
        public string Name { get; set; }
        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.Address")]
        public string Address { get; set; }
        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.PhoneNumber")]
        public string PhoneNumber { get; set; }
        [NopResourceDisplayName("AtrendUsa.Plugin.Misc.BuildYourBox.Email")]
        public string Email { get; set; }
    }

    //public enum MDFThicknessRequested
    //{
    //    [Display(Name = "Select MDF Thickness")]
    //    SelectMDFThickness,
    //    [Display(Name = "5/8”")]
    //    _5By8Inch,
    //    [Display(Name = "¾”")]
    //    _3By4Inch,
    //    [Display(Name = "1”")]
    //    _1Inch
    //}
}
