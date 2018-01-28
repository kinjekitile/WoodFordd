using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class VoucherAddCommand : ICommand {
        public VoucherModel Model { get; set; }
        
    }

    public class VoucherAddCommandHandler : ICommandHandler<VoucherAddCommand> {
        private readonly IVoucherService _voucherService;
        private readonly INotify _notify;
        private readonly IUserService _userService;
        private readonly ISettingService _settings;
        public VoucherAddCommandHandler(IVoucherService voucherService, INotify notify, IUserService userService, ISettingService settings) {
            _voucherService = voucherService;
            _notify = notify;
            _userService = userService;
            _settings = settings;
        }

        public void Handle(VoucherAddCommand command) {
            command.Model = _voucherService.Create(command.Model);

            if (!string.IsNullOrEmpty(command.Model.ClientEmail)) {
                string voucherPath = _settings.GetValue<string>(Setting.VoucherFileLocation);
                string templatePath = _settings.GetValue<string>(Setting.VoucherTemplateLocation);

                voucherPath = Path.Combine(voucherPath, command.Model.Id + ".jpg");

                byte[] voucherContents = _voucherService.GeneratorVoucherImage(command.Model, templatePath);

                using (Image image = Image.FromStream(new MemoryStream(voucherContents))) {
                    image.Save(voucherPath, ImageFormat.Jpeg);  // Or Png
                }

                UserVoucherNotificationModel model = new UserVoucherNotificationModel();
                UserModel user = new UserModel();
                user.Email = command.Model.ClientEmail;
                user.FirstName = command.Model.ClientName;
                model.User = user;
                model.Voucher = command.Model;
                model.HasAttachement = true;
                model.AttachmentPath = voucherPath;

                _notify.SendNotifyUserVoucher(model, Setting.Public_Website_Domain);
            }




        }
    }
}
