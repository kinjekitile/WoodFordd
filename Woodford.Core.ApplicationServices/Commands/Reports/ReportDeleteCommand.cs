using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ReportDeleteCommand : ICommand {
        public int Id { get; set; }               
    }

    public class ReportDeleteCommandHandler : ICommandHandler<ReportDeleteCommand> {
        private readonly IReportService _reportService;
        public ReportDeleteCommandHandler(IReportService reportService) {
            _reportService = reportService;
        }

        public void Handle(ReportDeleteCommand command) {

           
            _reportService.Delete(command.Id);
        }
    }
}
