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
    public class ImportBookingHistoryCommand : ICommand {
        public string Filename { get; set; }

        public byte[] FileContents { get; set; }
    }

    public class ImportBookingHistoryCommandHandler : ICommandHandler<ImportBookingHistoryCommand> {
        private readonly IDataImportService _dataImportService;
        private readonly ISettingService _settings;
        public ImportBookingHistoryCommandHandler(IDataImportService dataImportService, ISettingService settings) {
            _dataImportService = dataImportService;
            _settings = settings;
        }
        public void Handle(ImportBookingHistoryCommand command) {
            string backupFileFolder = _settings.GetValue<string>(Setting.DataExportLocalPath);
            string filenameAndPath = Path.Combine(backupFileFolder, command.Filename);
            using (FileStream fs = new FileStream(filenameAndPath, FileMode.Create)) {
                fs.Write(command.FileContents, 0, command.FileContents.Length);
            }
            
            _dataImportService.ImportExternalBookings(command.Filename);
        }
    }
}
