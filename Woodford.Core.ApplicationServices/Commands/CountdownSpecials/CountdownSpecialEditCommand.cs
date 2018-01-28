using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class CountdownSpecialEditCommand : ICommand {
        public CountdownSpecialModel Model { get; set; }        
    }

    public class CountdownSpecialEditCommandHandler : ICommandHandler<CountdownSpecialEditCommand> {
        private readonly ICountdownSpecialService _countdownSpecialService;
        public CountdownSpecialEditCommandHandler(ICountdownSpecialService countdownSpecialService) {
            _countdownSpecialService = countdownSpecialService;            
        }

        public void Handle(CountdownSpecialEditCommand command) {
            command.Model = _countdownSpecialService.Update(command.Model);
        }
    }
}
