using Nop.Core;
using Nop.Core.Domain.Messages;
using Nop.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtrendUsa.Plugin.Misc.BuildYourBox.Models;
using AtrendUsa.Plugin.Misc.BuildYourBox.Extensions;
using System.IO;
using System.IO.Compression;
using Nop.Services.BuildYourBoxPlugin;

namespace AtrendUsa.Plugin.Misc.BuildYourBox.Services
{
    public class BuildYourBoxMessageService : IBuildYourBoxMessageService
    {
        private readonly string _attachmentFileName = "attachments.zip";

        private readonly ITokenizer _tokenizer;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IStoreContext _storeContext;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly ICarpetColorService _carpetColorService;
        private readonly ITerminalTypeService _terminalTypeService;
        private readonly IMDFThicknessService _mDFThicknessService;

        private string msgId = string.Empty;

        public BuildYourBoxMessageService(
            ITokenizer tokenizer,
            IMessageTemplateService messageTemplateService,
            IQueuedEmailService queuedEmailService,
            IEmailAccountService emailAccountService,
            IStoreContext storeContext,
            EmailAccountSettings emailAccountSettings,
            ICarpetColorService CarpetColorService,
            ITerminalTypeService TerminalTypeService,
            IMDFThicknessService MDFThicknessService
            )
        {
            _tokenizer = tokenizer;
            _messageTemplateService = messageTemplateService;
            _queuedEmailService = queuedEmailService;
            _emailAccountService = emailAccountService;
            _storeContext = storeContext;
            _emailAccountSettings = emailAccountSettings;
            _carpetColorService = CarpetColorService;
            _terminalTypeService = TerminalTypeService;
            _mDFThicknessService = MDFThicknessService;
        }

        public void SendBuildYourBoxMessage(BuildYourBoxModel model)
        {
            var tokens = new List<Token>();

            tokens.Add(new Token("Store.Name", _storeContext.CurrentStore.Name));
            tokens.Add(new Token("BuildYourBoxModel.SubwooferManufacturer", model.SubwooferManufacturer));
            tokens.Add(new Token("BuildYourBoxModel.SubwooferModel", model.SubwooferModel));
            tokens.Add(new Token("BuildYourBoxModel.NumberOfSubwoofers", model.NumberOfSubwoofers));
            tokens.Add(new Token("BuildYourBoxModel.VehicleManufacturerOrModel", model.VehicleManufacturerOrModel));
            tokens.Add(new Token("BuildYourBoxModel.VehicleYear", model.VehicleYear));
            tokens.Add(new Token("BuildYourBoxModel.WidthRequired", model.WidthRequired));
            tokens.Add(new Token("BuildYourBoxModel.HeightRequired", model.HeightRequired));
            tokens.Add(new Token("BuildYourBoxModel.DepthRequired", model.DepthRequired));
            tokens.Add(new Token("BuildYourBoxModel.CarpetColorWanted", _carpetColorService.SelectAllCarpetColor().Where(x => x.Id == model.CarpetColorId).Select(x => x.CarpetColor).FirstOrDefault()));
            tokens.Add(new Token("BuildYourBoxModel.TerminalLocation", model.TerminalLocation));
            tokens.Add(new Token("BuildYourBoxModel.TerminalType", _terminalTypeService.SelectAllTerminalType().Where(x => x.Id == model.TerminalTypeId).Select(x => x.TerminalType).FirstOrDefault()));
            tokens.Add(new Token("BuildYourBoxModel.SealedOrVented", model.SealedOrVented));
            tokens.Add(new Token("BuildYourBoxModel.MDFThicknessRequested", _mDFThicknessService.SelectAllMDFThicknesses().Where(x => x.Id == model.MDFThicknessId).Select(x => x.MDFThickness).FirstOrDefault()));
            tokens.Add(new Token("BuildYourBoxModel.SquareOrAngledBox", model.SquareOrAngledBox));

            tokens.Add(new Token("BuildYourBoxModel.Name", model.Name));
            tokens.Add(new Token("BuildYourBoxModel.Address", model.Address));
            tokens.Add(new Token("BuildYourBoxModel.PhoneNumber", model.PhoneNumber));
            tokens.Add(new Token("BuildYourBoxModel.Email", model.Email));

            SendBuildYourBoxEmail(model, "BuildYourBox.BuildYourBoxForm", tokens);
        }

        public void SendBuildYourBoxEmail(BuildYourBoxModel model, string messageTemplateName, List<Token> messageBodyTokens)
        {
            var emailAccount = GetEmailAccount();

            var template = _messageTemplateService.GetMessageTemplateByName(messageTemplateName, _storeContext.CurrentStore.Id);
            var subject = GetSubject(template, model);
            string messageBody = _tokenizer.Replace(template.Body, messageBodyTokens, true);

            var email = new QueuedEmail
            {
                Priority = QueuedEmailPriority.High,
                From = emailAccount.Email, //Changed by IV Santosh to set From And To email from database only
                FromName = emailAccount.DisplayName,
                To = emailAccount.Email,
                //To = "santosh.ahire@ifuturz.com", //Added by IV Santosh
                ReplyTo = emailAccount.Email,
                ReplyToName = emailAccount.DisplayName,
                Subject = subject,
                Body = messageBody,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id
            };
            AddAttachemts(email, model);

            _queuedEmailService.InsertQueuedEmail(email);
        }

        private EmailAccount GetEmailAccount()
        {
            return _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
        }

        private string GetSubject(MessageTemplate template, BuildYourBoxModel model)
        {
            var storeName = _storeContext.CurrentStore.Name;
            if (string.IsNullOrEmpty(msgId))//Added By IV Santosh
            {
                msgId = GetMessageIdentifier();//Old
            }

            var subjectTokens = new List<Token>();
            subjectTokens.Add(new Token("Store.Name", storeName));
            subjectTokens.Add(new Token("BuildYourBoxModel.MessageNumber", msgId));

            return _tokenizer.Replace(template.Subject, subjectTokens, true);
        }

        private void AddAttachemts(QueuedEmail email, BuildYourBoxModel model)
        {
            string attachemtPath = CompressAttachments(model);

            if (!string.IsNullOrEmpty(attachemtPath))
            {
                email.AttachmentFilePath = attachemtPath;
                email.AttachmentFileName = Path.GetFileName(attachemtPath);
            }
        }

        private string CompressAttachments(BuildYourBoxModel model)
        {
            var attachments = model.ImageOfVehicleTrunkArea.Where(x => x != null);

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

        private string GetMessageIdentifier()
        {
            return DateTime.UtcNow.ToTimestamp().ToString();
        }
    }
}
