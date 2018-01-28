using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ReportEditCommand : ICommand {
        public ReportModel Model { get; set; }               
    }

    public class ReportEditCommandHandler : ICommandHandler<ReportEditCommand> {
        private readonly IReportService _reportService;
        public ReportEditCommandHandler(IReportService reportService) {
            _reportService = reportService;
        }

        public void Handle(ReportEditCommand command) {
            _reportService.Update(command.Model);
        }
    }
}
