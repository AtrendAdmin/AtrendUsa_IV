using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtrendUsa.Plugin.Misc.Support.Models;
using FluentValidation;
using Nop.Services.Localization;

namespace AtrendUsa.Plugin.Misc.Support.Validators
{
    public class BaseSupportModelValidator<T> : AbstractValidator<T> where T : BaseSupportModel
    {
        public BaseSupportModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.CompanyName) //Added By IV Santosh
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired")); //Added By IV Santosh
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
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
            RuleFor(x => x.Problem)
                .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
        }
    }
}
