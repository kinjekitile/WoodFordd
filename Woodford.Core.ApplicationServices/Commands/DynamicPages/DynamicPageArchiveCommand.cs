using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class DynamicPageArchiveCommand : ICommand {
        public int Id { get; set; }               
    }

    public class DynamicPageArchiveCommandHandler : ICommandHandler<DynamicPageArchiveCommand> {
        private readonly IDynamicPageService _dynamicPageService;
        public DynamicPageArchiveCommandHandler(IDynamicPageService dynamicPageService) {
            _dynamicPageService = dynamicPageService;
        }

        public void Handle(DynamicPageArchiveCommand command) {
            _dynamicPageService.Archive(command.Id);
        }
    }
}
