using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class DynamicPageAddCommand : ICommand {
        public DynamicPageModel Model { get; set; }               
    }

    public class DynamicPageAddCommandHandler : ICommandHandler<DynamicPageAddCommand> {
        private readonly IDynamicPageService _dynamicPageService;
        public DynamicPageAddCommandHandler(IDynamicPageService dynamicPageService) {
            _dynamicPageService = dynamicPageService;
        }

        public void Handle(DynamicPageAddCommand command) {
            command.Model = _dynamicPageService.Create(command.Model);
        }
    }
}
