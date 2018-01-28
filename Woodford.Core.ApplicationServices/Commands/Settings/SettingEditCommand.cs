using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class SettingEditCommand : ICommand {
        public Setting SettingName { get; set; }
        public string SettingValue { get; set; }
    }

    public class SettingEditCommandHandler : ICommandHandler<SettingEditCommand> {
        private readonly ISettingService _settingService;
        public SettingEditCommandHandler(ISettingService settingService) {
            _settingService = settingService;
        }

        public void Handle(SettingEditCommand command) {
            _settingService.SetValue(command.SettingName, command.SettingValue);            
        }
    }
}
