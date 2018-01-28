using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Providers;
using Woodford.Core.Interfaces.Services;

namespace Woodford.Core.ApplicationServices.Commands {
    public class EmailSignatureSendToUserCommand : ICommand {
        public int SignatureId { get; set; }
    }

    public class EmailSignatureSendToUserCommandHandler : ICommandHandler<EmailSignatureSendToUserCommand> {
        private readonly IEmailSignatureService _emailSignatureService;
        private readonly INotify _notify;
        private readonly ISettingService _settings;
        private readonly ICompressService _compressService;
        public EmailSignatureSendToUserCommandHandler(IEmailSignatureService emailSignatureService, INotify notify, ISettingService settings, ICompressService compressService) {
            _emailSignatureService = emailSignatureService;
            _notify = notify;
            _settings = settings;
            _compressService = compressService;
        }
        public void Handle(EmailSignatureSendToUserCommand command) {

            var signature = _emailSignatureService.GetById(command.SignatureId);


            string templateFolder = _settings.GetValue<string>(Setting.EmailSignatureTemplatesLocation);
            string outputRootFolder = _settings.GetValue<string>(Setting.EmailSignatureFolderOutputLocation);
            
            //Copy the template files to a new folder in output, folder name is user email
            string userFolder = Path.Combine(outputRootFolder, signature.Email);

            if (Directory.Exists(userFolder)) {
                Directory.Delete(userFolder, recursive: true);
            }

            Directory.CreateDirectory(userFolder);

            List<string> filenames = new List<string>();
            List<string> subFiles = new List<string>();


            filenames.Add("Woodford 2017 Dynamic.htm");
            filenames.Add("Woodford 2017 Dynamic.rtf");
            filenames.Add("Woodford 2017 Dynamic.txt");

            string templateSubPath = "Woodford 2017 Dynamic_files";

            subFiles.Add("colorschememapping.xml");
            subFiles.Add("filelist.xml");
            subFiles.Add("themedata.thmx");

            //Need to copy these over to the new directory
            foreach (var filename in filenames) {
                
                string sourcePath = Path.Combine(templateFolder, filename);
                string destinationPath = Path.Combine(userFolder, filename);

                File.Copy(sourcePath, destinationPath);

            }

            Directory.CreateDirectory(Path.Combine(userFolder, templateSubPath));

            foreach (var filename in subFiles) {

                string sourcePath = Path.Combine(templateFolder, templateSubPath, filename);
                string destinationPath = Path.Combine(userFolder, templateSubPath, filename);

                File.Copy(sourcePath, destinationPath);

            }

            //Now find and replace the signature id in the link within each of the main files

            foreach (var filename in filenames) {
                string fileToFindAndReplacePath = Path.Combine(userFolder, filename);

                string text = File.ReadAllText(fileToFindAndReplacePath);
                text = text.Replace("##SIGID##", signature.Id.ToString());
                File.WriteAllText(fileToFindAndReplacePath, text);
            }

            string outputPath = Path.Combine(outputRootFolder, signature.Email + ".zip");

            //Now zip the user folder 
            _compressService.CompressFolder(userFolder, outputPath);

            //Now email the zip as attachment to instructions 
            EmailSignatureToUserNotificationModel emailModel = new EmailSignatureToUserNotificationModel();
            emailModel.Signature = signature;
            emailModel.AttachmentPath = outputPath;
            emailModel.SiteDomain = _settings.GetValue<string>(Setting.Public_Website_Domain);

            _notify.SendNotifyEmployeeOfSignature(emailModel, Setting.Public_Website_Domain);
        }
    }
}
