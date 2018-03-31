using FluentValidation;
using Nop.Services.Localization;

namespace AtrendUsa.Plugin.Misc.Support.Models
{
    public class SupportModelValidator : AbstractValidator<SupportModel>
    {
        public SupportModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.StateProvince)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.City)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.AddressLine1)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));

            RuleFor(x => x.ManufacturerId)
                .GreaterThan(0).WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.ModelNumber)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.SerialNumber)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.Problem)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));

            RuleFor(x => x.DatePurchased)
                .NotEmpty().When(x => x.IsUnderWarranty).WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.Dealer)
                .NotEmpty().When(x => x.IsUnderWarranty).WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
        }
    }
}