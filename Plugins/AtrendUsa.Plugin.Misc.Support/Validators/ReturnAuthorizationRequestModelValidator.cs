using AtrendUsa.Plugin.Misc.Support.Models;
using FluentValidation;
using Nop.Services.Localization;

namespace AtrendUsa.Plugin.Misc.Support.Validators
{
    public class ReturnAuthorizationRequestModelValidator : BaseSupportModelValidator<ReturnAuthorizationRequestModel>
    {
        public ReturnAuthorizationRequestModelValidator(ILocalizationService localizationService) : base(
            localizationService)
        {
            RuleFor(x => x.ModelNumber)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.SerialNumber)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.DatePurchased)
                .NotEmpty().When(x => x.IsUnderWarranty).WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.Dealer)
                .NotEmpty().When(x => x.IsUnderWarranty).WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
        }
    }
}
