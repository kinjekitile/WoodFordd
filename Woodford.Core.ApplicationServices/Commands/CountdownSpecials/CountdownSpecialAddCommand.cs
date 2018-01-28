using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class CountdownSpecialAddCommand : ICommand {
        public CountdownSpecialModel Model { get; set; }        
    }

    public class CountdownSpecialAddCommandHandler : ICommandHandler<CountdownSpecialAddCommand> {
        private readonly ICountdownSpecialService _countdownSpecialService;
        public CountdownSpecialAddCommandHandler(ICountdownSpecialService countdownSpecialService) {
            _countdownSpecialService = countdownSpecialService;            
        }

        public void Handle(CountdownSpecialAddCommand command) {
            command.Model = _countdownSpecialService.Create(command.Model);
        }
    }
}
