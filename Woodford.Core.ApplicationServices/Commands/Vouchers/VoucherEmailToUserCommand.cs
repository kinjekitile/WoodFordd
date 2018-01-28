using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands.Vouchers {
    public class VoucherEmailToUserCommand : ICommand {
        public int VoucherId { get; set; }
        public string Email { get; set; }
        public string CustomerName { get; set; }
        public bool OUTRedeemed { get; set; }

    }

    public class VoucherEmailToUserCommandHandler : ICommandHandler<VoucherEmailToUserCommand> {
        private readonly IVoucherService _voucherService;
        private readonly INotify _notify;
        private readonly IUserService _userService;
        private readonly ISettingService _settings;

        public VoucherEmailToUserCommandHandler(IVoucherService voucherService, INotify notify, IUserService userService, ISettingService settings) {
            _voucherService = voucherService;
            _notify = notify;
            _userService = userService;
            _settings = settings;
        }

        public void Handle(VoucherEmailToUserCommand command) {



            var voucher = _voucherService.GetById(command.VoucherId);

            string emailTo = command.Email;

            if (voucher.IsMultiUse) {
                emailTo = command.Email;
            }
            else {
                //Check if redeemed
                if (voucher.DateRedeemed.HasValue) {
                    command.OUTRedeemed = true;
                    return;
                }
                else {
                    //Can only send to email set against voucher
                    emailTo = voucher.ClientEmail;

                }
            }

            //Email address set and is not redeemed or is multiuse
            string voucherPath = _settings.GetValue<string>(Setting.VoucherFileLocation);
            string templatePath = _settings.GetValue<string>(Setting.VoucherTemplateLocation);

            voucherPath = Path.Combine(voucherPath, voucher.Id + ".jpg");
            if (File.Exists(voucherPath)) {
                //exclamation key broken, sorry
            } else {
                byte[] voucherContents = _voucherService.GeneratorVoucherImage(voucher, templatePath);

                using (Image image = Image.FromStream(new MemoryStream(voucherContents))) {
                    image.Save(voucherPath, ImageFormat.Jpeg);  // Or Png
                }
            }

            UserVoucherNotificationModel model = new UserVoucherNotificationModel();
            UserModel user = new UserModel();
            user.Email = command.Email;
            user.FirstName = command.CustomerName;
            model.User = user;
            model.Voucher = voucher;
            model.HasAttachement = true;
            model.AttachmentPath = voucherPath;

            _notify.SendNotifyUserVoucher(model, Setting.Public_Website_Domain);

        }
    }
}
