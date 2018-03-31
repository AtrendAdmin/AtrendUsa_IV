using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Web.Models.Common;

namespace Nop.Web.Validators.Common
{
    public class NowCommerceSignUpValidator : BaseNopValidator<NowCommerceSignUpModel>
    {
        public NowCommerceSignUpValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ContactEmail).NotEmpty().WithMessage(localizationService.GetResource("NowCommerceSignUp.ContactEmail.Required"));
            RuleFor(x => x.ContactEmail).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));

            RuleFor(x => x.ContactPerson).NotEmpty().WithMessage(localizationService.GetResource("NowCommerceSignUp.ContactPerson.Required"));

            RuleFor(x => x.DealerName).NotEmpty().WithMessage(localizationService.GetResource("NowCommerceSignUp.DealerName.Required"));
        }
    }
}