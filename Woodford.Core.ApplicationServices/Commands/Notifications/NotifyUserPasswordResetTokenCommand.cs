using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class NotifyUserPasswordResetTokenCommand : ICommand {
        public string Email { get; set; }
    }

    public class NotifyUserPasswordResetTokenCommandHandler : ICommandHandler<NotifyUserPasswordResetTokenCommand> {

        private readonly IAuthenticate _authenticate;
        private readonly INotify _notify;
        private readonly ISettingService _settingService;

        public NotifyUserPasswordResetTokenCommandHandler(IAuthenticate authenticate, INotify notify, ISettingService settingService) {
            _authenticate = authenticate;
            _notify = notify;
            _settingService = settingService;
        }

        public void Handle(NotifyUserPasswordResetTokenCommand command) {

            string resetToken = _authenticate.GeneratePasswordResetToken(command.Email);
            string siteDomain = _settingService.GetValue<string>(Setting.Public_Website_Domain);
            ForgotPasswordNotificationModel model = new ForgotPasswordNotificationModel {                
                ResetToken = resetToken,
                Email = command.Email,
                SiteDomain = siteDomain
            };

            _notify.SendNotifyUserPasswordReset(model, Setting.Public_Website_Domain);
            //_notify.AddMailToQueue(command.Email, from, subject, model, NotificationResources.ForgotPassword, Setting.Public_Website_Domain);

        }
    }
}
