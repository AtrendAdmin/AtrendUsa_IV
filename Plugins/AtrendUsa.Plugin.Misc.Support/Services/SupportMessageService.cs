using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AtrendUsa.Plugin.Misc.Support.Extensions;
using AtrendUsa.Plugin.Misc.Support.Models;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Messages;
using Nop.Services.Directory;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Stores;

namespace AtrendUsa.Plugin.Misc.Support.Services
{
    public class SupportMessageService : ISupportMessageService
    {
        private readonly string _attachmentFileName = "attachments.zip";

        private readonly ICountryService _countryService;
        private readonly ITokenizer _tokenizer;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IStoreContext _storeContext;
        private readonly EmailAccountSettings _emailAccountSettings;
        private string msgId = string.Empty; //Added By IV Santosh

        public SupportMessageService(
            ICountryService countryService,
            ITokenizer tokenizer,
            IMessageTemplateService messageTemplateService,
            IQueuedEmailService queuedEmailService,
            IEmailAccountService emailAccountService,
            IStoreContext storeContext,
            EmailAccountSettings emailAccountSettings)
        {
            _countryService = countryService;
            _tokenizer = tokenizer;
            _messageTemplateService = messageTemplateService;
            _queuedEmailService = queuedEmailService;
            _emailAccountService = emailAccountService;
            _storeContext = storeContext;
            _emailAccountSettings = emailAccountSettings;
        }

        public void SendFreightOrderClaimMessage(FreightOrderClaimModel model)
        {
            var bodyTokens = GetCommonTokens(model);
            bodyTokens.Add(new Token("SupportModel.ModelNumber", model.ModelNumber));
            bodyTokens.Add(new Token("SupportModel.SerialNumber", model.SerialNumber));
            bodyTokens.Add(new Token("FreightOrderClaimModel.ExceptionType", model.ExceptionTypeName));

            SendSupportEmail(model, "Support.FreightOrderClaim", bodyTokens);
            SendSupportEmailToCustomer(model, "Support.FreightOrderClaimCustomerMsg", bodyTokens); //Added By IV Santosh
        }

        public void SendReturnAuthorizationRequestMessage(ReturnAuthorizationRequestModel model)
        {
            var bodyTokens = GetCommonTokens(model);
            bodyTokens.Add(new Token("SupportModel.ModelNumber", model.ModelNumber));
            bodyTokens.Add(new Token("SupportModel.SerialNumber", model.SerialNumber));
            bodyTokens.Add(new Token("ReturnAuthorizationRequestModel.UnderWarranty", model.IsUnderWarranty ? "Yes" : "No"));
            bodyTokens.Add(new Token("ReturnAuthorizationRequestModel.DatePurchased", model.DatePurchased));
            bodyTokens.Add(new Token("ReturnAuthorizationRequestModel.Dealer", model.Dealer));

            SendSupportEmail(model, "Support.ReturnAuthorizationRequest", bodyTokens);
        }

        private string GetSubject(MessageTemplate template, BaseSupportModel model)
        {
            var storeName = _storeContext.CurrentStore.Name;
            if (string.IsNullOrEmpty(msgId))//Added By IV Santosh
            {
                msgId = GetMessageIdentifier();//Old
            }

            var subjectTokens = new List<Token>();
            subjectTokens.Add(new Token("Store.Name", storeName));
            subjectTokens.Add(new Token("Support.FullName", model.FullName));
            subjectTokens.Add(new Token("Support.MessageNumber", msgId));

            return _tokenizer.Replace(template.Subject, subjectTokens, true);
        }

        private void SendSupportEmail(BaseSupportModel model, string messageTemplateName, List<Token> messageBodyTokens)
        {
            var emailAccount = GetEmailAccount();

            var template = _messageTemplateService.GetMessageTemplateByName(messageTemplateName, _storeContext.CurrentStore.Id);
            var subject = GetSubject(template, model);
            string messageBody = _tokenizer.Replace(template.Body, messageBodyTokens, true);

            var email = new QueuedEmail
            {
                Priority = QueuedEmailPriority.High,
                //From = model.Email, //Commented by IV Santosh
                From = emailAccount.Email, //Changed by IV Santosh to set From And To email from database only
                //FromName = emailAccount.DisplayName, //Commeted by IV Santosh to change Diaplay name as customer first and last name
                FromName = string.Format("{0} {1} - {2}", model.FirstName, model.LastName, model.Email),
                To = string.Format("{0};{1};{2}", emailAccount.Email, "pawel@atrendusa.com", "armando@atrendusa.com"),

                //To = "hasmukh.savaliya@ifuturz.com", //Added by IV Santosh
                ReplyTo = model.Email,
                ReplyToName = model.FullName,
                Subject = subject,
                Body = messageBody,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id
            };
            AddAttachemts(email, model);

            _queuedEmailService.InsertQueuedEmail(email);
        }
        /// <summary>
        /// Sends a claim email to customer from marketing@atrendusa.com
        /// </summary>
        /// <param name="model">Claim support email</param>
        /// <param name="messageTemplateName">Template name from Message Template from Management</param>
        /// <param name="messageBodyTokens">Body tokes with data to fill with email message body</param>
        private void SendSupportEmailToCustomer(BaseSupportModel model, string messageTemplateName, List<Token> messageBodyTokens)//Added by IV Santosh 
        {
            var emailAccount = GetEmailAccount();

            var template = _messageTemplateService.GetMessageTemplateByName(messageTemplateName, _storeContext.CurrentStore.Id);
            var subject = GetSubject(template, model);
            messageBodyTokens.Add(new Token("Support.MessageNumber", msgId));
            string messageBody = _tokenizer.Replace(template.Body, messageBodyTokens, true);

            var email = new QueuedEmail
            {
                Priority = QueuedEmailPriority.High,
                From = emailAccount.Email,
                FromName = emailAccount.DisplayName,
                To = model.Email,
                ReplyTo = emailAccount.Email,
                ReplyToName = emailAccount.DisplayName,
                Subject = subject,
                Body = messageBody,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id
            };
            //AddAttachemts(email, model);

            _queuedEmailService.InsertQueuedEmail(email);
        }

        private void AddAttachemts(QueuedEmail email, BaseSupportModel model)
        {
            string attachemtPath = CompressAttachments(model);

            if (!string.IsNullOrEmpty(attachemtPath))
            {
                email.AttachmentFilePath = attachemtPath;
                email.AttachmentFileName = Path.GetFileName(attachemtPath);
            }
        }

        private string CompressAttachments(BaseSupportModel model)
        {
            var attachments = model.UploadedDocuments.Where(x => x != null);

            if (!attachments.Any())
            {
                return null;
            }

            // TODO: clear created directory?
            var guid = Guid.NewGuid().ToString();
            string directoryPath = CommonHelper.MapPath($"~/Content/files/attachments/{guid}");
            Directory.CreateDirectory(directoryPath);

            foreach (var file in attachments)
            {
                if (file.ContentLength > 0)
                {
                    file.SaveAs(Path.Combine(directoryPath, file.FileName));
                }
            }

            var zipDir = Path.Combine(Path.GetTempPath(), guid);
            Directory.CreateDirectory(zipDir);
            var zipFilePath = Path.Combine(zipDir, _attachmentFileName);
            ZipFile.CreateFromDirectory(directoryPath, zipFilePath);

            return zipFilePath;
        }

        private EmailAccount GetEmailAccount()
        {
            return _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
        }

        private List<Token> GetCommonTokens(BaseSupportModel model)
        {
            var tokens = GetBaseSupportModelTokens(model);
            tokens.Add(new Token("Store.Name", _storeContext.CurrentStore.Name));
            return tokens;
        }

        private List<Token> GetBaseSupportModelTokens(BaseSupportModel model)
        {
            var tokens = new List<Token>();

            tokens.Add(new Token("SupportModel.CompanyName", model.CompanyName));//Added By IV Santosh
            tokens.Add(new Token("SupportModel.FirstName", model.FirstName));
            tokens.Add(new Token("SupportModel.LastName", model.LastName));
            tokens.Add(new Token("SupportModel.Email", model.Email));
            tokens.Add(new Token("SupportModel.StateProvince", model.StateProvince));
            tokens.Add(new Token("SupportModel.City", model.City));
            tokens.Add(new Token("SupportModel.AddressLine1", model.AddressLine1));
            tokens.Add(new Token("SupportModel.AddressLine2", model.AddressLine2));
            tokens.Add(new Token("SupportModel.PostalCode", model.PostalCode));
            tokens.Add(new Token("SupportModel.Phone", model.Phone));
            tokens.Add(new Token("SupportModel.Problem", model.Problem));

            if (model.CountryId.HasValue)
            {
                tokens.Add(new Token("SupportModel.Country", _countryService.GetCountryById(model.CountryId.Value).Name));
            }

            return tokens;
        }

        private string GetMessageIdentifier()
        {
            return DateTime.UtcNow.ToTimestamp().ToString();
        }
    }
}
