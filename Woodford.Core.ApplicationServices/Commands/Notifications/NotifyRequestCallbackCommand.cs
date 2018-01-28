using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class NotifyRequestCallbackCommand : ICommand {
        public RequestCallbackNotificationModel RequestCallback { get; set; }
    }

    public class NotifyRequestCallbackCommandHandler : ICommandHandler<NotifyRequestCallbackCommand> {
        private readonly INotify _notify;
        private readonly ISettingService _settingService;
        public NotifyRequestCallbackCommandHandler(INotify notify, ISettingService settingService) {
            _notify = notify;
            _settingService = settingService;
        }

        public void Handle(NotifyRequestCallbackCommand command) {            
            _notify.SendNotifyRequestCallback(command.RequestCallback, Setting.Public_Website_Domain);
        }
    }
}
