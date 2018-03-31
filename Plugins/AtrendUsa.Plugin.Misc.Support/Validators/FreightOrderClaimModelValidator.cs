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
    public class FreightOrderClaimModelValidator : BaseSupportModelValidator<FreightOrderClaimModel>
    {
        public FreightOrderClaimModelValidator(ILocalizationService localizationService) : base(localizationService)
        {
            //RuleFor(x => x.ModelNumber)         //Commented by IV Santosh
            //    .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.ModelNumberId)
               .NotEmpty().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
            RuleFor(x => x.ExceptionType)
                .NotNull().WithMessage(localizationService.GetResource("AtrendUsa.Plugin.Misc.Support.FieldRequired"));
        }
    }
}
