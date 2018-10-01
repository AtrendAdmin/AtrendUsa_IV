using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using AtrendUsa.Plugin.Misc.BuildYourBox.Models;
using Nop.Services.Localization;

namespace AtrendUsa.Plugin.Misc.BuildYourBox.Validators
{
    class BuildYourBoxModelValidator<T> : AbstractValidator<T> where T : BuildYourBoxModel
    {
        public BuildYourBoxModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.SubwooferManufacturer)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.SubwooferModel)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.NumberOfSubwoofers)
                .GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.VehicleManufacturerOrModel)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.VehicleYear)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.ImageOfVehicleTrunkArea)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.WidthRequired)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.HeightRequired)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.DepthRequired)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.CarpetColorId)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.TerminalLocation)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.TerminalTypeId)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.SealedOrVented)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.MDFThicknessId)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.SquareOrAngledBox)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.Name)
               .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.Address)
              .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.PhoneNumber)
              .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));

            RuleFor(x => x.Email)
              .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired"));
        }
    }
}
