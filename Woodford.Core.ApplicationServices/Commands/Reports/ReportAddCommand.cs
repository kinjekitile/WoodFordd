using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ReportAddCommand : ICommand {
        public ReportModel Model { get; set; }               
    }

    public class ReportAddCommandHandler : ICommandHandler<ReportAddCommand> {
        private readonly IReportService _reportService;
        public ReportAddCommandHandler(IReportService reportService) {
            _reportService = reportService;
        }

        public void Handle(ReportAddCommand command) {

            if (command.Model.ReportType == ReportType.User) {
                //User reports are historical, therefor swap start and end date logic
                
            }
            command.Model = _reportService.Create(command.Model);
        }
    }
}
