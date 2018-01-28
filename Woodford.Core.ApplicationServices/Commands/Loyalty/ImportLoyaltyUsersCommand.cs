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

namespace Woodford.Core.ApplicationServices.Commands {
    public class ImportLoyaltyUsersCommand : ICommand {
        public string Filename { get; set; }

        
    }

    public class ImportLoyaltyUsersCommandHandler : ICommandHandler<ImportLoyaltyUsersCommand> {
        private readonly IDataImportService _dataImportService;
        private readonly ISettingService _settings;
        public ImportLoyaltyUsersCommandHandler(IDataImportService dataImportService, ISettingService settings) {
            _dataImportService = dataImportService;
            _settings = settings;
        }
        public void Handle(ImportLoyaltyUsersCommand command) {
            string backupFileFolder = _settings.GetValue<string>(Setting.DataExportLocalPath);
            string filenameAndPath = Path.Combine(backupFileFolder, command.Filename);
            
            _dataImportService.ImportAdvanceUsers(command.Filename);
        }
    }
}
