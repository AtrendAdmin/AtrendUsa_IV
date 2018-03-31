using FluentValidation;
using Nop.Core.Domain.Common;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Web.Models.Common;

namespace Nop.Web.Validators.Common
{
    public partial class ContactUsValidator : BaseNopValidator<ContactUsModel>
    {
        public ContactUsValidator(ILocalizationService localizationService, CommonSettings commonSettings)
        {
            RuleFor(x => x.ActgEmail).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.Email.Required"));
            RuleFor(x => x.ActgEmail).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.BuyerEmail).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.Email.Required"));
            RuleFor(x => x.BuyerEmail).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.FirstName.Required"));
            RuleFor(x => x.LastName).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.LastName.Required"));
            RuleFor(x => x.AdditionalComments).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.AdditionalComments.Required"));
            RuleFor(x => x.CompanyName).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.CompanyName.Required"));
            RuleFor(x => x.BusinessAddress).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.Address.Required"));
            RuleFor(x => x.HomeAddress).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.Address.Required"));
            RuleFor(x => x.Title).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.Title.Required"));
            RuleFor(x => x.City).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.City.Required"));
            RuleFor(x => x.StateProvince).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.State.Required"));
            RuleFor(x => x.ZipCode).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.ZipCode.Required"));
            RuleFor(x => x.BusinessPhoneNumber).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.PhoneNumber.Required"));
            RuleFor(x => x.FaxNumber).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.PhoneNumber.Required"));
            RuleFor(x => x.HomePhoneNumber).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.PhoneNumber.Required"));
            RuleFor(x => x.AccountingContact).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.PhoneNumber.Required"));
            // RuleFor(x => x.AcceptedTermsAndConditions).NotEqual(false).WithMessage(localizationService.GetResource("ContactUs.AcceptedTermsAndConditions.Required"));
            RuleFor(x => x.TypeOfBusiness).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.TypeOfBusiness.Required"));
            RuleFor(x => x.Established).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.Established.Required"));
            RuleFor(x => x.KeyInterest).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.KeyInterest.Required"));
            RuleFor(x => x.Country).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.Country.Required"));

            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.FullName).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.FullName.Required"));
            if (commonSettings.SubjectFieldOnContactUsForm)
            {
                RuleFor(x => x.Subject).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.Subject.Required"));
            }
            RuleFor(x => x.Enquiry).NotEmpty().WithMessage(localizationService.GetResource("ContactUs.Enquiry.Required"));
        }}
}