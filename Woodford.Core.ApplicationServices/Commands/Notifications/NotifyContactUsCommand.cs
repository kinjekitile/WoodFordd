using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class NotifyContactUsCommand : ICommand {
        public ContactUsNotificationModel Contact { get; set; }
    }

    public class NotifyContactUsCommandHandler : ICommandHandler<NotifyContactUsCommand> {
        private readonly INotify _notify;
        private readonly ISettingService _settingService;
        public NotifyContactUsCommandHandler(INotify notify, ISettingService settingService) {
            _notify = notify;
            _settingService = settingService;
        }

        public void Handle(NotifyContactUsCommand command) {            
            _notify.SendNotifyContactUs(command.Contact, Setting.Public_Website_Domain);
        }
    }
}
