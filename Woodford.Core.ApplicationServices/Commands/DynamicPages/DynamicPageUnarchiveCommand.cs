using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class DynamicPageUnarchiveCommand : ICommand {
        public int Id { get; set; }               
    }

    public class DynamicPageUnarchiveCommandHandler : ICommandHandler<DynamicPageUnarchiveCommand> {
        private readonly IDynamicPageService _dynamicPageService;
        public DynamicPageUnarchiveCommandHandler(IDynamicPageService dynamicPageService) {
            _dynamicPageService = dynamicPageService;
        }

        public void Handle(DynamicPageUnarchiveCommand command) {
            _dynamicPageService.UnArchive(command.Id);
        }
    }
}
